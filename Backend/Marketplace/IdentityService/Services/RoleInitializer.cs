using IdentityService.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("seller") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (await roleManager.FindByNameAsync("consumer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("consumer"));
            }
        }
    }
}
