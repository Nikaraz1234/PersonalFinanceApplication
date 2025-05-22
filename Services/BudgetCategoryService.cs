using AutoMapper;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Services
{
    public class BudgetCategoryService : IBudgetCategoryService
    {
        private readonly IBudgetCategoryRepository _budgetCategoryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public BudgetCategoryService(
            IBudgetCategoryRepository budgetCategoryRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _budgetCategoryRepository = budgetCategoryRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<BudgetCategoryDTO> CreateBudgetCategoryAsync(CreateBudgetCategoryDTO categoryDto)
        {
            var category = _mapper.Map<BudgetCategory>(categoryDto);
            category.CurrentSpending = 0;

            var createdCategory = await _budgetCategoryRepository.CreateBudgetCategoryAsync(category);
            return await GetBudgetCategoryWithDetailsAsync(createdCategory.Id);
        }

        public async Task DeleteBudgetCategoryAsync(int id)
        {
            await _budgetCategoryRepository.DeleteBudgetCategoryAsync(id);
        }

        public async Task<BudgetCategoryDTO> GetBudgetCategoryAsync(int id)
        {
            return await GetBudgetCategoryWithDetailsAsync(id);
        }

        public async Task<IEnumerable<BudgetCategoryDTO>> GetBudgetCategoriesByBudgetAsync(int budgetId)
        {
            var categories = await _budgetCategoryRepository.GetBudgetCategoriesByBudgetAsync(budgetId);
            var categoryDtos = new List<BudgetCategoryDTO>();

            foreach (var category in categories)
            {
                categoryDtos.Add(await GetBudgetCategoryWithDetailsAsync(category.Id));
            }

            return categoryDtos;
        }

        public async Task<BudgetCategoryDTO> UpdateBudgetCategoryAsync(UpdateBudgetCategoryDTO categoryDto)
        {
            var existingCategory = await _budgetCategoryRepository.GetBudgetCategoryAsync(categoryDto.Id);
            if (existingCategory == null)
            {
                return null;
            }

            _mapper.Map(categoryDto, existingCategory);

            var updatedCategory = await _budgetCategoryRepository.UpdateBudgetCategoryAsync(existingCategory);
            return await GetBudgetCategoryWithDetailsAsync(updatedCategory.Id);
        }

        public async Task UpdateCurrentSpendingAsync(int categoryId, decimal amount)
        {
            await _budgetCategoryRepository.UpdateCurrentSpendingAsync(categoryId, amount);
        }

        private async Task<BudgetCategoryDTO> GetBudgetCategoryWithDetailsAsync(int id)
        {
            var category = await _budgetCategoryRepository.GetBudgetCategoryAsync(id);
            if (category == null)
            {
                return null;
            }

            var categoryDto = _mapper.Map<BudgetCategoryDTO>(category);

            // Get recent transactions (last 3)
            var transactions = category.Transactions
                .OrderByDescending(t => t.Date)
                .Take(3)
                .ToList();

            categoryDto.RecentTransactions = _mapper.Map<List<TransactionDTO>>(transactions);

            return categoryDto;
        }
    }


}
