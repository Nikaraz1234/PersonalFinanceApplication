using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetDTO> GetBudgetByIdAsync(int id);
        Task<IEnumerable<BudgetSummaryDTO>> GetUserBudgetsAsync(int userId);
        Task<BudgetDTO> CreateBudgetAsync(CreateBudgetDTO budgetDto);
        Task<BudgetDTO> UpdateBudgetAsync(int id, UpdateBudgetDTO budgetDto);
        Task<bool> DeleteBudgetAsync(int id);
        Task<IEnumerable<BudgetCategoryDTO>> GetBudgetCategoriesAsync(int budgetId);
    }
}
