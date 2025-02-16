using IdentityService.Models.Identity;
using IdentityService.Repositories.AppUserRepositories;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Api.Account
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private IAppUserRepository _appUserRepository;

        public AccountController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetApplicationUser(string id)
        {
            ApplicationUser? appUser = await _appUserRepository.GetAppUserByIdAsync(id);
            if (appUser == null) 
                return NotFound();

            return Ok(new
            {
                id = appUser.Id,
                userType = appUser.UserType,
                name = appUser.UserName,
                description = appUser.Description,
            });
        }
    }
}
