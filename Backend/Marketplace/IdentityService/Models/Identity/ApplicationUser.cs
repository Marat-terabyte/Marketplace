using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserType UserType { get; set; }
        public string? Description { get; set; }
    }
}
