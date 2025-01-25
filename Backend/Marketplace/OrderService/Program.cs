using OrderService.Extensions;
using Marketplace.Shared.Config.Middleware;
using ProductService.JsonConverters;
using OrderService.Services;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            Console.WriteLine(DateTime.Now);

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ObjectIdConverter());
            });

            builder.Configuration.AddJsonFile("jwt.json");
            builder.Services.AddJwt(builder.Configuration);

            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddMongoRepositories();
            builder.Services.AddScoped<OrderManager>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
