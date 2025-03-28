using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets.Categories
{
    public class CreateBudgetCategoryDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal AllocatedAmount { get; set; }
    }
}
