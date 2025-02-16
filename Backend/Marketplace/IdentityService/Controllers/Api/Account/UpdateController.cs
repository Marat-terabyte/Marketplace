using IdentityService.Models.Identity;
using IdentityService.Repositories.AppUserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityService.Models.InputModels;

namespace IdentityService.Controllers.Api.Account
{
    [Route("/api/account/update")]
    [ApiController]
    public class UpdateController : Controller
    {
        private IAppUserRepository _appUserRepository;

        public UpdateController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        [HttpPut]
        [Route("description")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> UpdateDescription([FromBody] UpdateDescriptionInput input)
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized();

            ApplicationUser? appUser = await _appUserRepository.GetAppUserByIdAsync(id);
            if (appUser == null)
                return NotFound();

            appUser.Description = input.Description;

            await _appUserRepository.UpdateUserAsync(appUser);

            return Ok(new 
            { 
                id = appUser.Id 
            });
        }
    }
}
