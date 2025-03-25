namespace PersonalFinanceApplication.Models
{
    public class RecurringBill
    {
        public int Id { get; set; } 
        public int UserId { get; set; }  
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; } 
        public User User { get; set; } 

    }
}
