using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _autoMapper;

        public UserRepository(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {


            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithBudgetsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Budgets)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserWithTransactionsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserWithSavingsPotsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.SavingsPots)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdatePasswordHashAsync(int userId, string passwordHash)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.PasswordHash = passwordHash;
                await UpdateAsync(user);
            }
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                await UpdateAsync(user);
            }
        }

        public Task SetPasswordResetTokenAsync(string email, string token, DateTime expires)
        {
            throw new NotImplementedException();
        }
    }
}