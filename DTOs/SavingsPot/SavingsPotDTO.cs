using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.SavingsPot
{
    public class SavingsPotDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public DateTime? TargetDate { get; set; }

        public int UserId { get; set; }

        public List<SavingsTransactionDto> Transactions { get; set; } = new();

    }


}
