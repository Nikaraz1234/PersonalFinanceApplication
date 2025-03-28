using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    public class BillPayment
    {
        public int Id { get; set; }

        [Required, Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public bool IsPaid { get; set; } = false;

        public int RecurringBillId { get; set; }
        public RecurringBill RecurringBill { get; set; }
    }
}
