using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.RecurringBill
{
    public class UpdateRecurringBillDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0.01, 1000000)]
        public decimal? Amount { get; set; }

        [StringLength(20)]
        public string? Frequency { get; set; }

        public DateTime? DueDate { get; set; }

        public bool? IsPaid { get; set; }
    }

}
