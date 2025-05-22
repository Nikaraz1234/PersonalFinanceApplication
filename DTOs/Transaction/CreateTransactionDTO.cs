using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        public string? Description { get; set; }
        [Required]
        public string Category { get; set; }
        public int? BudgetCategoryId { get; set; }

        [Required]
        public int UserId { get; set; }
    }

}
