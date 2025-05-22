using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    public class RecurringBill
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, Range(1, 31, ErrorMessage = "Due day must be between 1 and 31")]
        public int DueDay { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string? Category { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<BillPayment> Payments { get; set; } = new List<BillPayment>();
    }
}
