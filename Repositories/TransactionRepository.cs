using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.DTOs.Pagination;


namespace PersonalFinanceApplication.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.BudgetCategory)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.BudgetCategory)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.BudgetCategory)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> SearchTransactionsAsync(int userId, string searchTerm, DateTime? startDate, DateTime? endDate, int? categoryId,
        string sortBy = "Date",
        string sortDirection = "desc")
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.BudgetCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Description.Contains(searchTerm));
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date <= endDate.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.BudgetCategoryId == categoryId.Value);
            }
            if (sortDirection.ToLower() == "asc")
            {
                query = sortBy.ToLower() switch
                {
                    "date" => query.OrderBy(t => t.Date),
                    "amount" => query.OrderBy(t => t.Amount),
                    "description" => query.OrderBy(t => t.Description),
                    _ => query.OrderBy(t => t.Date) 
                };
            }
            else if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "date" => query.OrderByDescending(t => t.Date),
                    "amount" => query.OrderByDescending(t => t.Amount),
                    "description" => query.OrderByDescending(t => t.Description),
                    _ => query.OrderByDescending(t => t.Date)
                };
            }
            return await query.ToListAsync();
        }

        public async Task<bool> TransactionExists(int id)
        {
            return await _context.Transactions.AnyAsync(t => t.Id == id);
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(int categoryId)
        {
            return await _context.Transactions
                .Where(t => t.BudgetCategoryId == categoryId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }
        public async Task<PaginatedResult<Transaction>> GetUserTransactionsPagedAsync(int userId, PaginationParams pagination)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.BudgetCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pagination.SearchTerm))
                query = query.Where(t => t.Description.Contains(pagination.SearchTerm));

            if (pagination.CategoryId.HasValue)
                query = query.Where(t => t.BudgetCategoryId == pagination.CategoryId.Value);

            query = pagination.SortBy?.ToLower() switch
            {
                "amount" => pagination.IsDescending ? query.OrderByDescending(t => t.Amount) : query.OrderBy(t => t.Amount),
                "date" => pagination.IsDescending ? query.OrderByDescending(t => t.Date) : query.OrderBy(t => t.Date),
                _ => query.OrderByDescending(t => t.Date)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<Transaction>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
        public async Task<List<Transaction>> GetLatestThreeTransactionsPerCategoryAsync()
        {
            var transactions = await _context.Transactions
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return transactions
                .GroupBy(t => t.Category)
                .SelectMany(g => g.Take(3))
                .ToList();
        }

        public async Task<Dictionary<string, decimal>> GetMonthlySpendingAsync()
        {
            return await _context.Transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:00}",
                    Total = g.Sum(t => t.Amount)
                })
                .ToDictionaryAsync(x => x.Month, x => x.Total);
        }

    }
}
