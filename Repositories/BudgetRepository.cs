using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class BudgetRepository :IBudgetRepository
    {
        private readonly AppDbContext _context;

        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBudgetAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBudgetAsync(int id)
        {
            var budget = await GetBudgetByIdAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Budget with Id {id} not found.");
            }
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int userId)
        {
            var budgets = await _context.Budgets.Where(b => b.UserId == userId).ToListAsync();
            return budgets;
        }

        public async Task UpdateBudgetAsync(int id, BudgetDTO budgetDto)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if(budget == null)
            {
                throw new Exception($"Budget with Id {budgetDto.Id} not found.");
            }

            budget.Category = budgetDto.Category;
            budget.Limit = budgetDto.Limit;
            budget.CurrentSpending = budgetDto.CurrentSpending;

            await _context.SaveChangesAsync();
            
        }

        
    }
}
