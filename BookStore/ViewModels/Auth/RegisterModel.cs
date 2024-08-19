using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels.Auth
{
    public class RegisterModel : LoginModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
