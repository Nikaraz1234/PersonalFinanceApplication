using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class SavingsPotRepository : ISavingsPotRepository
    {
        private readonly AppDbContext _context;

        public SavingsPotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SavingsPot>> GetAllAsync()
            => await _context.SavingsPots.ToListAsync();

        public async Task<SavingsPot> GetByIdAsync(int id)
            => await _context.SavingsPots.Include(p => p.Transactions).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<SavingsPot>> GetByUserIdAsync(int userId)
            => await _context.SavingsPots.Where(p => p.UserId == userId).Include(p => p.Transactions).ToListAsync();

        public async Task AddAsync(SavingsPot pot)
            => await _context.SavingsPots.AddAsync(pot);

        public async Task UpdateAsync(SavingsPot pot)
            => _context.SavingsPots.Update(pot);

        public async Task DeleteAsync(SavingsPot pot)
        {
            var user = await _context.Users.FindAsync(pot.UserId);

            user.MainBalance += pot.CurrentAmount;

            _context.SavingsPots.Remove(pot);
        }
            

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
