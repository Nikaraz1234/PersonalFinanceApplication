using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetCategoryRepository
    {
        Task<BudgetCategory> GetBudgetCategoryAsync(int id);
        Task<IEnumerable<BudgetCategory>> GetBudgetCategoriesByBudgetAsync(int budgetId);
        Task<BudgetCategory> CreateBudgetCategoryAsync(BudgetCategory category);
        Task<BudgetCategory> UpdateBudgetCategoryAsync(BudgetCategory category);
        Task DeleteBudgetCategoryAsync(int id);
        Task<bool> BudgetCategoryExists(int id);
        Task UpdateCurrentSpendingAsync(int categoryId, decimal amount);
    }
}
