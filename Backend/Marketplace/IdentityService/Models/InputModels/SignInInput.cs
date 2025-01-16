using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.InputModels
{
    public class SignInInput
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidation")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
    }
}
