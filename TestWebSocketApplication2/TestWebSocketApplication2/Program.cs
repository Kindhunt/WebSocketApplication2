using Microsoft.AspNetCore.Builder;
using TestWebSocketApplication2;

var builder = new AppConfigServices(args).Builder;
var app = builder.Build();

app.UseWebSockets();

app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.Map("/ws", wsApp => 
{
    wsApp.Use(async (context, next) => 
    {
        if (context.WebSockets.IsWebSocketRequest) {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        }
        else {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        await next(context);
    });
});


app.UseEndpoints(configure: endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Login",
        pattern: "{controller=Auth}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
        name: "Index",
        pattern: "{controller=Auth}/{action=Index}/{id?}");
});

app.Run();
