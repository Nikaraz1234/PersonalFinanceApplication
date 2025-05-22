using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Transaction
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public int? BudgetCategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
