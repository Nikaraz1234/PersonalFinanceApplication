using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class SavingsTransactionRepository : ISavingsTransactionRepository
    {
        private readonly AppDbContext _context;

        public SavingsTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SavingsTransaction>> GetByPotIdAsync(int savingsPotId)
            => await _context.SavingsTransactions.Where(t => t.SavingsPotId == savingsPotId).ToListAsync();

        public async Task<SavingsTransaction> GetByIdAsync(int id)
            => await _context.SavingsTransactions.FindAsync(id);

        public async Task AddAsync(SavingsTransaction transaction)
            => await _context.SavingsTransactions.AddAsync(transaction);

        public async Task DeleteAsync(SavingsTransaction transaction)
            => _context.SavingsTransactions.Remove(transaction);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
