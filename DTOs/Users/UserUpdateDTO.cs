using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? NewEmail { get; set; }

        [MinLength(8)]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        public string? ConfirmNewPassword { get; set; }

        [Required]
        public string CurrentPassword { get; set; }
    }
}
