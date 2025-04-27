using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Budget name must be under 100 characters")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be between $0.01 and $1,000,000")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<BudgetCategoryDTO> Categories { get; set; } = new();

        [Range(0, 1000000)]
        public decimal CurrentSpending { get; set; }

        [Range(0, 100)]
        public decimal ProgressPercentage =>
            TotalAmount > 0 ? (CurrentSpending / TotalAmount) * 100 : 0;

    }
}
