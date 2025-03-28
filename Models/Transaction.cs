using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [Required, Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        public bool IsRecurring { get; set; } = false;

        public int? BudgetCategoryId { get; set; }
        public BudgetCategory? BudgetCategory { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
