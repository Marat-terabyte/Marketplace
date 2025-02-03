using CartService.Extensions;
using Marketplace.Shared.Config.Middleware;
using ProductService.JsonConverters;

namespace CartService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("jwt.json");
            
            builder.Services.AddJwt(builder.Configuration);
            
            builder.Services.AddSingleton(_ => new Config(builder.Configuration));
            builder.Services.AddScoped<HttpClient>();

            builder.Services.AddMongoDbServices(builder.Configuration);
            await builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ObjectIdConverter());
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
