using IdentityService.Models.Identity;
using IdentityService.Models.InputModels;
using IdentityService.Services;
using Marketplace.Shared.Formatters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Api.Seller
{
    [ApiController]
    [Route("api/seller")]
    public class SignUpController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public SignUpController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Index([FromBody] SellerSignUp input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ErrorFormater.Create(ModelState);

                return BadRequest(errors);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = input.Email,
                UserName = input.Storename!.Trim(),
                UserType = UserType.Seller
            };

            var result = await _userManager.CreateAsync(user, input.Password!);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "seller");

            return Ok();
        }
    }
}
