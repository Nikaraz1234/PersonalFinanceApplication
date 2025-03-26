using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget> GetBudgetByIdAsync(int id);
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int userId);
        Task AddBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(int id, BudgetDTO budgetDto);
        Task DeleteBudgetAsync(int id);
    }
}
