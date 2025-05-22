using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets.Categories
{
    public class UpdateBudgetCategoryDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [Range(0.01, 1000000)]
        public decimal? AllocatedAmount { get; set; }
    }
}
