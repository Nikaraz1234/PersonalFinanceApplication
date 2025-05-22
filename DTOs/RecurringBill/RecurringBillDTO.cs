using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.RecurringBill
{
    public class RecurringBillDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; } // e.g., "Monthly"
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
