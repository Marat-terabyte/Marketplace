using ProductService.Extensions;
using ProductService.Services;
using Microsoft.AspNetCore.Mvc;
using ProductService.JsonConverters;
using Marketplace.Shared.Config.Middleware;
using ProductService.Services.BackgroundServices;

namespace ProductService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ObjectIdConverter());
            });


            builder.Services.AddMongoDbServices(builder.Configuration);
            builder.Services.AddCache(builder.Configuration);
            builder.Services.AddScoped<ItemService>();
            builder.Services.AddScoped<ProductStockService>();

            builder.Configuration.AddJsonFile("jwt.json");
            builder.Services.AddJwt(builder.Configuration);

            await builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddHostedService<ProductCountDecreaser>();
            builder.Services.AddHostedService<ProductCountRestorer>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
