using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.InputModels
{
    public class SellerSignUp
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidation")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "StorenameRequired")]
        public string Storename { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "ConfirmPasswordMismatch")]
        public string ConfirmPassword { get; set; }
    }
}
