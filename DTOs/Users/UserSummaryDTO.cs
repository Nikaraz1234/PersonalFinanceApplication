using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserSummaryDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public string Initials =>
            $"{FirstName?.FirstOrDefault()}{LastName?.FirstOrDefault()}".ToUpper();
        public bool IsActive => LastLogin.HasValue && LastLogin.Value > DateTime.UtcNow.AddMonths(-6);
    }
}
