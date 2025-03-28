using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApplication.Models
{
    public class SavingsPot
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; } = 0;

        public DateTime? TargetDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<SavingsTransaction> Transactions { get; set; } = new List<SavingsTransaction>();
    }
}
