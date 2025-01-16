using IdentityService.Models.Identity;
using IdentityService.Models.InputModels;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Api.Account
{
    [ApiController]
    [Route("/api/account/")]
    public class SignInController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private TokenService _tokenService;

        public SignInController(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login([FromBody] SignInInput input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ErrorFormater.Create(ModelState);
                
                return BadRequest(errors);
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                return Unauthorized();

            IList<string> roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.Generate(user, roles);

            return Json(
                new
                {
                    token = token,
                    expiration = DateTime.Now.AddHours(5)
                }
            );
        }
    }
}
