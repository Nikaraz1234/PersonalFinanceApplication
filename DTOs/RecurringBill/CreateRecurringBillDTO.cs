using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.RecurringBill
{
    public class CreateRecurringBillDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string Frequency { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int UserId { get; set; }
    }

}
