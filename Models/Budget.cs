namespace PersonalFinanceApplication.Models
{
    public class Budget
    {
        public int Id { get; set; }  
        public int UserId { get; set; } 
        public string Category { get; set; }
        public decimal Limit { get; set; }
        public decimal CurrentSpending { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}
