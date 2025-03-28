using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
