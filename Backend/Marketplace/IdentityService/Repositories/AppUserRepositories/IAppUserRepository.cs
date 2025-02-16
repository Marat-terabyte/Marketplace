using IdentityService.Models.Identity;

namespace IdentityService.Repositories.AppUserRepositories
{
    public interface IAppUserRepository
    {
        Task<ApplicationUser?> GetAppUserByIdAsync(string id);
    }
}
