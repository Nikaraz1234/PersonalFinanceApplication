using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserRegisterDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain uppercase, lowercase, and number")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }
    }
}
