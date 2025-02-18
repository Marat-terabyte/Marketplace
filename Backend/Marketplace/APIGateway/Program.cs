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

            CorsConfig cors = builder.Configuration.GetSection("CorsConfig").Get<CorsConfig>() 
                ?? throw new ArgumentNullException("'CorsConfig' field in appsettings.json is empty");

            if (cors.Origins == null || cors.Origins.Length == 0)
                throw new ArgumentNullException("'CorsConfig:Origins' field in appsettings.json is empty");

            builder.Services.AddCors(
                options =>
                {
                    options.AddPolicy("DefaultPolicy", p =>
                    {
                        p.WithOrigins(cors.Origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                }
            );

            builder.Services.AddSignalR();
            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseCors("DefaultPolicy");

            await app.UseOcelot();

            app.Run();
        }
    }
}
