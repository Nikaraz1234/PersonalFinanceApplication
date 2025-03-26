using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetService
    {
        Task<Budget> GetBudgetByIdAsync(int id);
        Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId);
        Task<Budget> CreateBudgetAsync(Budget budget);
        Task<BudgetDTO> UpdateBudgetAsync(int id, BudgetDTO budgetDto);
        Task<bool> DeleteBudgetAsync(int id);
    }
}
