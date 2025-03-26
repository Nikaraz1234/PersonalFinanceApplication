namespace PersonalFinanceApplication.DTOs
{
    public class SavingsPotDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
