using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ISavingsPotRepository
    {
        Task<IEnumerable<SavingsPot>> GetAllAsync();
        Task<SavingsPot> GetByIdAsync(int id);
        Task<IEnumerable<SavingsPot>> GetByUserIdAsync(int userId);
        Task AddAsync(SavingsPot pot);
        Task UpdateAsync(SavingsPot pot);
        Task DeleteAsync(SavingsPot pot);
        Task SaveChangesAsync();
    }
}
