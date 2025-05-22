using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.SavingsPot
{
    public class UpdateSavingsPotDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, Range(0.01, 1000000)]
        public decimal TargetAmount { get; set; }

        public DateTime? TargetDate { get; set; }
    }

}
