using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Repositories;

namespace PersonalFinanceApplication.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly BudgetRepository _repo;

        public BudgetService(BudgetRepository repo)
        {
            _repo = repo;
        }

        public async Task<Budget> CreateBudgetAsync(Budget budget)
        {
            await _repo.AddBudgetAsync(budget);
            return budget;
        }

        public async Task<bool> DeleteBudgetAsync(int id)
        {
            await _repo.DeleteBudgetAsync(id);
            return true;
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            return await _repo.GetBudgetByIdAsync(id);
            
        }

        public async Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId)
        {
            return await _repo.GetBudgetsByUserIdAsync(userId);
        }

        public async Task<BudgetDTO> UpdateBudgetAsync(int id, BudgetDTO budgetDto)
        {
            await _repo.UpdateBudgetAsync(id, budgetDto);
            return budgetDto;
        }
    }
}
