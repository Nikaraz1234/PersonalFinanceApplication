using PersonalFinanceApplication.DTOs.Budgets.Categories;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IBudgetCategoryService
    {
        Task<BudgetCategoryDTO> GetBudgetCategoryAsync(int id);
        Task<IEnumerable<BudgetCategoryDTO>> GetBudgetCategoriesByBudgetAsync(int budgetId);
        Task<BudgetCategoryDTO> CreateBudgetCategoryAsync(CreateBudgetCategoryDTO categoryDto);
        Task<BudgetCategoryDTO> UpdateBudgetCategoryAsync(UpdateBudgetCategoryDTO categoryDto);
        Task DeleteBudgetCategoryAsync(int id);
        Task UpdateCurrentSpendingAsync(int categoryId, decimal amount);
    }
}
