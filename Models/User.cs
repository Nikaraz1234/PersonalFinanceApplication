using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }
        [Range(0, double.MaxValue)]
        public decimal MainBalance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<SavingsPot> SavingsPots { get; set; }
        public ICollection<RecurringBill> RecurringBills { get; set; }

        public string PasswordResetToken { get; set; } = string.Empty;
        public DateTime? PasswordResetExpires { get; set; }
    }
}
