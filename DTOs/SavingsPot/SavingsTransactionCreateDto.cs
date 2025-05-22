using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.SavingsPot
{
    public class SavingsTransactionCreateDto
    {
        [Required, Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(20)]
        [RegularExpression("Deposit|Withdraw", ErrorMessage = "TransactionType must be either 'Deposit' or 'Withdraw'.")]
        public string TransactionType { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [Required]
        public int SavingsPotId { get; set; }
    }
}
