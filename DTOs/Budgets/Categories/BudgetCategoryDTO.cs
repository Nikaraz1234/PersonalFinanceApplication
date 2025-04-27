using PersonalFinanceApplication.DTOs.Transaction;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets.Categories
{
    public class BudgetCategoryDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0.01, 1000000)]
        public decimal AllocatedAmount { get; set; }

        public decimal CurrentSpending { get; set; }

        [Range(0, 100)]
        public decimal UtilizationPercentage =>
            AllocatedAmount > 0 ? (CurrentSpending / AllocatedAmount) * 100 : 0;

        public List<TransactionDTO> RecentTransactions { get; set; } = new();

    }
}
