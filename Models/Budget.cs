using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } // e.g., "October 2023 Budget"

        [Required, Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        [NotMapped] 
        public decimal CurrentSpending =>
        Transactions?.Sum(t => t.Amount) ?? 0;

        public ICollection<BudgetCategory> Categories { get; set; } = new List<BudgetCategory>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
