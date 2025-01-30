using Marketplace.Shared.Config.Middleware;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models.Repositories;

namespace PaymentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("jwt.json");
            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite") ??
                throw new NullReferenceException("SQL connection string is empty")));

            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();

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
