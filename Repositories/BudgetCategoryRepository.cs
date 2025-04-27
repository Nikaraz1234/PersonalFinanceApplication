using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class BudgetCategoryRepository : IBudgetCategoryRepository
    {
        private readonly AppDbContext _context;

        public BudgetCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BudgetCategory> GetBudgetCategoryAsync(int id)
        {
            return await _context.BudgetCategories
                .Include(c => c.Budget)
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<BudgetCategory>> GetBudgetCategoriesByBudgetAsync(int budgetId)
        {
            return await _context.BudgetCategories
                .Where(c => c.BudgetId == budgetId)
                .Include(c => c.Transactions)
                .ToListAsync();
        }

        public async Task<BudgetCategory> CreateBudgetCategoryAsync(BudgetCategory category)
        {
            _context.BudgetCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<BudgetCategory> UpdateBudgetCategoryAsync(BudgetCategory category)
        {
            _context.BudgetCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteBudgetCategoryAsync(int id)
        {
            var category = await _context.BudgetCategories.FindAsync(id);
            if (category != null)
            {
                _context.BudgetCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> BudgetCategoryExists(int id)
        {
            return await _context.BudgetCategories.AnyAsync(c => c.Id == id);
        }

        public async Task UpdateCurrentSpendingAsync(int categoryId, decimal amount)
        {
            var category = await _context.BudgetCategories.FindAsync(categoryId);
            if (category != null)
            {
                category.CurrentSpending += amount;
                await _context.SaveChangesAsync();
            }
        }
    }
}
