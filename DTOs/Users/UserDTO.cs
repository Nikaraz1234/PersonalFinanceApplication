using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        // Calculated property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
