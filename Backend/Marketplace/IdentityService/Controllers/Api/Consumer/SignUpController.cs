using IdentityService.Models.Identity;
using IdentityService.Models.InputModels;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers.Api.Consumer
{
    [ApiController]
    [Route("/api/consumer")]
    public class SignUpController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public SignUpController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register([FromBody] ConsumerSignUp input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ErrorFormater.Create(ModelState);

                return BadRequest(errors);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = input.Email,
                UserName = $"{input.Surname} {input.Name} {input.Patronymic}".Trim(),
            };

            var result = await _userManager.CreateAsync(user, input.Password!);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "consumer");

            return Ok();
        }
    }
}
