using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.SavingsPot
{
    public class SavingsTransactionDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string TransactionType { get; set; } 

        public string Notes { get; set; }

        public int SavingsPotId { get; set; }
    }

}
