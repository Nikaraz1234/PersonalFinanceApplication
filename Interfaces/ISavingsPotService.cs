using PersonalFinanceApplication.DTOs.SavingsPot;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ISavingsPotService
    {
        Task<List<SavingsPotDTO>> GetAllByUserAsync(int userId);
        Task<SavingsPotDTO> GetByIdAsync(int id);
        Task<SavingsPotDTO> CreateAsync(CreateSavingsPotDTO dto);
        Task<SavingsPotDTO> UpdateAsync(int id, UpdateSavingsPotDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<SavingsTransactionDto> AddTransactionAsync(SavingsTransactionCreateDto dto);
        Task<List<SavingsTransactionDto>> GetTransactionsByPotIdAsync(int potId);
        Task<decimal> GetCurrentBalanceAsync(int id);
    }
}
