using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<bool> Exists(string user);
        Task<bool> Exists(int id);
        Task<bool> EmailExists(string email);

        Task<User> GetUserWithBudgetsAsync(int id);
        Task<User> GetUserWithTransactionsAsync(int id);
        Task<User> GetUserWithSavingsPotsAsync(int id);
        Task<User> GetByUsernameAsync(string username);


        Task UpdatePasswordHashAsync(int userId, string passwordHash);
        Task UpdateLastLoginAsync(int userId);

        Task SetPasswordResetTokenAsync(string email, string token, DateTime expires);
    }
}
