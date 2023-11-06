using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System;
using TestWebSocketApplication2.Models;
using Newtonsoft.Json;

namespace TestWebSocketApplication2.Handlers
{
	public static class WebSocketManager
	{
		private static HashSet<WebSocket> _webSockets = new HashSet<WebSocket>();

		public static void AddWebSocket(WebSocket webSocket)
		{
			_webSockets.Add(webSocket);
		}

		public static void RemoveWebSocket(WebSocket webSocket)
		{
			_webSockets.Remove(webSocket);
		}
		public static async Task SendMessage(object _obj, WebSocket webSocket)
		{
			try
			{
				string jsonString = JsonConvert.SerializeObject(_obj);
				byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
				await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
			}
			catch (Exception ex)
			{
				throw new Exception($"Some problem when send message, here is error: {ex.Message}");
			}
		}
		public static async Task<T> ReceiveMessage<T>(WebSocket webSocket)
		{
			try {
				byte[] buffer = new byte[1024];
				WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				string jsonString = string.Empty;
				if (result.MessageType == WebSocketMessageType.Text) {
					jsonString = Encoding.UTF8.GetString(buffer);
				}
				return JsonConvert.DeserializeObject<T>(jsonString);
			}
			catch (Exception ex) {
				throw new Exception($"Some problem when receive message, here is error: {ex.Message}");
			}
		}

		public static async Task Authenticating(object _obj)
		{
			if (_obj.GetType() == typeof(User))
			{
				var user = (User)_obj;

				var options = new DbContextOptionsBuilder<ApplicationDbContext>()
					.UseSqlServer(System.Configuration.ConfigurationManager.AppSettings["DatabaseConnection"])
					.Options;

				using (var context = new ApplicationDbContext(options))
				{
					var userFromDB = context.Users
						.Where(v => v.Email.Equals(user.Email) && v.Password.Equals(user.Password))
						.FirstOrDefault();
					if (userFromDB != null)
					{
						Console.WriteLine("Data is correct");
					}
					else 
					{ 
						throw new Exception("Data is uncorrect (email/password)"); 
					}
				}
			}

			foreach (var webSocket in _webSockets) {
				try
				{
					if (webSocket.State == WebSocketState.Open)
					{
						await SendMessage(_obj, webSocket);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
	public class WebSocketHandler
	{
		public async Task HandleWebSocket(WebSocket webSocket)
		{
			Console.WriteLine("Handling new websocket connection...");

			WebSocketManager.AddWebSocket(webSocket);

			try
			{
				byte[] buffer = new byte[1024];
				while (webSocket.State == WebSocketState.Open)
				{
					Console.WriteLine("Receiving message...");
					var user = await WebSocketManager.ReceiveMessage<User>(webSocket);

					Console.WriteLine("Authenticating...");
					await WebSocketManager.Authenticating(user);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				WebSocketManager.RemoveWebSocket(webSocket);
				webSocket.Dispose();
			}
			finally
			{
				WebSocketManager.RemoveWebSocket(webSocket);
				webSocket.Dispose();
			}
		}
	}
}
