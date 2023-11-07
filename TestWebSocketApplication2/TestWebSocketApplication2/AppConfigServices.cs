using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace TestWebSocketApplication2
{
    public class AppConfigServices
    {
        public WebApplicationBuilder Builder { get; set; }
        public AppConfigServices(string[] args) {
            ConfigureServices(args);
        }
        public void ConfigureServices(string[] args)
        {
            Builder = WebApplication.CreateBuilder(args);
            Builder.Services.AddDbContext<ApplicationDbContext>();
			Builder.Services.AddMvc().AddControllersAsServices();

            Builder.WebHost.UseKestrel(serverOptions =>
            {
                serverOptions.Listen(
                    System.Net.IPAddress.Parse(
                        System.Configuration.ConfigurationManager.AppSettings["HttpsAddress"]), 
                    int.Parse(System.Configuration.ConfigurationManager.AppSettings["HttpsPort"]),
                    listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            });
		}
	}
}
