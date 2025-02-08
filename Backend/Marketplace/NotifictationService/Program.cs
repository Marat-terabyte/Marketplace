using Marketplace.Shared.Config.Middleware;
using NotifictationService.Services.BackgroundServices;
using NotifictationService.Hubs;

namespace NotifictationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("jwt.json");

            builder.Services.AddSignalR();
            
            builder.Services.AddControllers();

            builder.Services.AddJwt(builder.Configuration);

            await builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddHostedService<NotificationSender>();

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

            app.MapHub<NotificationHub>("/hub/notification");
            app.MapControllers();

            app.Run();
        }
    }
}
