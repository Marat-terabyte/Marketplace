using Ocelot;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;


namespace APIGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("ocelot.json");

            builder.Services.AddSignalR();
            builder.Services.AddOcelot();

            var app = builder.Build();

            await app.UseOcelot();

            app.Run();
        }
    }
}
