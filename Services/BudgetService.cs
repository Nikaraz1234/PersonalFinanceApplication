using AutoMapper;
using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Repositories;

namespace PersonalFinanceApplication.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repo;
        private readonly IMapper _mapper;

        public BudgetService(IBudgetRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BudgetDTO> CreateBudgetAsync(CreateBudgetDTO budgetDto)
        {
            var budget = _mapper.Map<Budget>(budgetDto);
            await _repo.AddBudgetAsync(budget);
            return _mapper.Map<BudgetDTO>(budget);
        }

        public async Task<bool> DeleteBudgetAsync(int id)
        {
            await _repo.DeleteBudgetAsync(id);
            return true;
        }

        public async Task<BudgetDTO> GetBudgetByIdAsync(int id)
        {
            var budget = await _repo.GetBudgetByIdAsync(id);
            return _mapper.Map<BudgetDTO>(budget);
        }

        public async Task<IEnumerable<BudgetCategoryDTO>> GetBudgetCategoriesAsync(int budgetId)
        {
            var budget = await _repo.GetBudgetByIdAsync(budgetId);
            return _mapper.Map<List<BudgetCategoryDTO>>(budget.Categories);
        }

        public async Task<IEnumerable<BudgetSummaryDTO>> GetUserBudgetsAsync(int userId)
        {
            var budgets = await _repo.GetUserBudgetsAsync(userId);
            return _mapper.Map<List<BudgetSummaryDTO>>(budgets);
        }

        public async Task<BudgetDTO> UpdateBudgetAsync(int id, UpdateBudgetDTO budgetDto)
        {
            var existingBudget = await _repo.GetBudgetByIdAsync(id);
            _mapper.Map(budgetDto, existingBudget);
            await _repo.UpdateBudgetAsync(existingBudget);
            return _mapper.Map<BudgetDTO>(existingBudget);
        }
    }
}
