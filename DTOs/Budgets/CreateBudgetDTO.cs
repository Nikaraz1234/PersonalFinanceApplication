using PersonalFinanceApplication.DTOs.Budgets.Categories;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets
{
    public class CreateBudgetDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public List<CreateBudgetCategoryDTO> Categories { get; set; } = new();
    }
}
