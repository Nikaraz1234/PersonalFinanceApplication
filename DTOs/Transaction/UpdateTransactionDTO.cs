using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Transaction
{
    public class UpdateTransactionDTO
    {
        [Required]
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [Range(0.01, 1000000)]
        public decimal? Amount { get; set; }

        public string? Description { get; set; }

        public int? BudgetCategoryId { get; set; }
    }
}
