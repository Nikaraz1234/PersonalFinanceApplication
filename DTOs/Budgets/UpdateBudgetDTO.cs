using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Budgets
{
    public class UpdateBudgetDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0.01, 1000000)]
        public decimal? TotalAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
