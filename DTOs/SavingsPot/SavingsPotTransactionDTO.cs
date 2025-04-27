using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.SavingsPot
{
    public class SavingsPotTransactionDTO
    {
        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public bool IsDeposit { get; set; } // true = add, false = withdraw
    }

}
