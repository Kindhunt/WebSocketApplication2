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
        }
    }
}
