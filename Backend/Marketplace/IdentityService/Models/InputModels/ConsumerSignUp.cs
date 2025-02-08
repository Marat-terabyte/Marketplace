using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace IdentityService.Models.InputModels
{
    public class ConsumerSignUp
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidation")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SurnameRequired")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "ConfirmPasswordMismatch")]
        public string ConfirmPassword { get; set; }
    }
}
