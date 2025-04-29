using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ISavingsTransactionRepository
    {
        Task<IEnumerable<SavingsTransaction>> GetByPotIdAsync(int savingsPotId);
        Task<SavingsTransaction> GetByIdAsync(int id);
        Task AddAsync(SavingsTransaction transaction);
        Task DeleteAsync(SavingsTransaction transaction);
        Task SaveChangesAsync();
    }
}

