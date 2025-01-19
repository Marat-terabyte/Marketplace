using ProductService.Extensions;
using ProductService.Services;
using Microsoft.AspNetCore.Mvc;
using ProductService.JsonConverters;
using Marketplace.Shared.Config.Middleware;

namespace ProductService
{
    public class Program
    {
        public static void Main(string[] args)
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

            builder.Configuration.AddJsonFile("jwt.json");
            builder.Services.AddJwt(builder.Configuration);

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
