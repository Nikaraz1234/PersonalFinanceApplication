using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget> GetBudgetByIdAsync(int id);
        Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId);
        Task AddBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int id);
        Task<IEnumerable<Budget>> GetBudgetsByCategoryAsync(int budgetId, string category);
    }
}
