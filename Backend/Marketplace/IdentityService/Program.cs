using IdentityService.Data;
using IdentityService.Extensions;
using IdentityService.Models.Identity;
using IdentityService.Repositories.AppUserRepositories;
using IdentityService.Services;
using Marketplace.Shared.Config.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("jwt.json");

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite") ??
                throw new NullReferenceException("SQL connection string is empty")));
            builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

            builder.Services.AddAspIdentity();
            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddScoped<TokenService>();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            InitializeDb(scope.ServiceProvider);

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static async void InitializeDb(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            await RoleInitializer.InitializeAsync(userManager, roleManager);
        }
    }
}
