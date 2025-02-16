using IdentityService.Data;
using IdentityService.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repositories.AppUserRepositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private ApplicationContext _db;

        public AppUserRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<ApplicationUser?> GetAppUserByIdAsync(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(ApplicationUser appUser)
        {
            _db.Users.Update(appUser);
            await _db.SaveChangesAsync();
        }
    }
}
