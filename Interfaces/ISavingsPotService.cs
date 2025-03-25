using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ISavingsPotService
    {
        Task<SavingsPot> GetSavingsPotByIdAsync(int id);
        Task<IEnumerable<SavingsPot>> GetUserSavingsPotsAsync(int userId);
        Task<SavingsPot> CreateSavingsPotAsync(SavingsPot savingsPot);
        Task<SavingsPot> UpdateSavingsPotAsync(SavingsPot savingsPot);
        Task<bool> DeleteSavingsPotAsync(int id);
        Task<bool> AddMoneyToPotAsync(int potId, decimal amount);
        Task<bool> WithdrawMoneyFromPotAsync(int potId, decimal amount);

    }
}
