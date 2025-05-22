using PersonalFinanceApplication.DTOs.Budgets.Categories;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets
{
    public class BudgetSummaryDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal CurrentSpending { get; set; }

        public string Period =>
            $"{StartDate:MMM d} - {EndDate:MMM d, yyyy}";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public decimal ProgressPercentage { get; set; }

        public string Status =>
            DateTime.Now > EndDate ? "Completed" :
            DateTime.Now >= StartDate ? "Active" : "Upcoming";

    }
}
