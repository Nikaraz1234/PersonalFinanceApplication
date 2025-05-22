using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApplication.DTOs.Users
{
    public class UserProfileDTO : UserDTO
    {
        public decimal TotalBalance { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyExpenses { get; set; }

        public List<UserBudgetSummaryDTO> ActiveBudgets { get; set; } = new();
        public List<UserAccountDTO> LinkedAccounts { get; set; } = new();

        public DateTime? SubscriptionExpiry { get; set; }
        public bool EmailVerified { get; set; }
    }

    public class UserBudgetSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ProgressPercentage { get; set; }
    }

    public class UserAccountDTO
    {
        public string AccountName { get; set; }
        public string LastFourDigits { get; set; }
        public string AccountType { get; set; } 
    }
}
