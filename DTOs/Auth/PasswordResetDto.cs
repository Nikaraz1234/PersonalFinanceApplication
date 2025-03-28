using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
        public string NewPassword { get; set; }

        // Added confirmation field
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
