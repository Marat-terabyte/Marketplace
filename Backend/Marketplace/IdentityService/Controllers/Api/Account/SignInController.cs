using IdentityService.Models.Identity;
using IdentityService.Models.InputModels;
using IdentityService.Services;
using Marketplace.Shared.Formatters;
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

            DateTime expirationTime = DateTime.Now.AddHours(24);

            CookieOptions cookie = new CookieOptions{
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = expirationTime
            };

            Response.Cookies.Append("jwt", token, cookie);

            return Json(
                new
                {
                    id = user.Id,
                    token = token,
                    name = user.UserName,
                    role = roles,
                    expiration = expirationTime
                }
            );
        }
    }
}
