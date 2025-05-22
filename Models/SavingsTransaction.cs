
using PersonalFinanceApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    
    public class SavingsTransaction
    {
        public int Id { get; set; }

        [Required, Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(20)]
        public string TransactionType { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public int SavingsPotId { get; set; }
        public SavingsPot SavingsPot { get; set; }
    }
}
