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

            AddOcelotJson(builder);
            CorsConfig cors = GetCorsConfig(builder.Configuration);

            builder.Services.AddCors(
                options =>
                {
                    options.AddPolicy("DefaultPolicy", p =>
                    {
                        p.WithOrigins(cors.Origins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                }
            );

            builder.Services.AddSignalR();
            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseCors("DefaultPolicy");

            app.Use((async (context, next) =>
            {
                if (context.Request.Method == HttpMethod.Options.Method)
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
                    context.Response.Headers.Append("Access-Control-Max-Age", "86400");

                    context.Response.StatusCode = 200;
                    await context.Response.CompleteAsync();
                }
                else
                {
                    await next.Invoke();
                }
            }));

            await app.UseOcelot();

            app.Run();
        }

        private static void AddOcelotJson(WebApplicationBuilder builder)
        {
            if (builder.Environment.EnvironmentName == "Container")
                builder.Configuration.AddJsonFile("ocelot.Container.json");
            else
                builder.Configuration.AddJsonFile("ocelot.json");
        }

        private static CorsConfig GetCorsConfig(ConfigurationManager config)
        {
            CorsConfig cors = config.GetSection("CorsConfig").Get<CorsConfig>()
                ?? throw new ArgumentNullException("'CorsConfig' or 'CorsConfig:Origins' field in appsettings.json are empty");

            return cors;
        }
    }
}
