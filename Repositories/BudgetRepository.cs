using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BudgetRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            return await _context.Budgets
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByCategoryAsync(int budgetId, string category)
        {
            return await _context.Budgets
                .Where(b => b.Id == budgetId)
                .Include(b => b.Categories.Where(bc => bc.Name == category))
                .ToListAsync();
        }

        public async Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId)
                .Include(b => b.Categories)
                .ToListAsync();
        }

        public async Task UpdateBudgetAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }
    }
}
