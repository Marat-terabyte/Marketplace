using IdentityService.Models.Identity;
using IdentityService.Repositories.AppUserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Api.Account
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private IAppUserRepository _appUserRepository;

        public AccountController(IAppUserRepository appUserRepository, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
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

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("jwt");

            return Ok();
        }
    }
}
