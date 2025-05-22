using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets.Categories
{
    public class CreateBudgetCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Allocated amount is required")]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
        public decimal AllocatedAmount { get; set; }

        [Required(ErrorMessage = "Budget ID is required")]
        public int BudgetId { get; set; }
    }
}
