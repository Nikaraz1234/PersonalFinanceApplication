using AutoMapper;
using PersonalFinanceApplication.DTOs.SavingsPot;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using Supabase.Gotrue;

namespace PersonalFinanceApplication.Services
{
    public class SavingsPotService : ISavingsPotService
    {
        private readonly ISavingsPotRepository _potRepo;
        private readonly ISavingsTransactionRepository _transactionRepo;
        private readonly IMapper _mapper;

        public SavingsPotService(
            ISavingsPotRepository potRepo,
            ISavingsTransactionRepository transactionRepo,
            IMapper mapper)
        {
            _potRepo = potRepo;
            _transactionRepo = transactionRepo;
            _mapper = mapper;
        }

        public async Task<List<SavingsPotDTO>> GetAllByUserAsync(int userId)
        {
            var pots = await _potRepo.GetByUserIdAsync(userId);
            return _mapper.Map<List<SavingsPotDTO>>(pots);
        }

        public async Task<SavingsPotDTO> GetByIdAsync(int id)
        {
            var pot = await _potRepo.GetByIdAsync(id);
            return _mapper.Map<SavingsPotDTO>(pot);
        }

        public async Task<SavingsPotDTO> CreateAsync(CreateSavingsPotDTO dto)
        {
            var pot = _mapper.Map<SavingsPot>(dto);
            await _potRepo.AddAsync(pot);
            await _potRepo.SaveChangesAsync();
            return _mapper.Map<SavingsPotDTO>(pot);
        }

        public async Task<SavingsPotDTO> UpdateAsync(int id, UpdateSavingsPotDTO dto)
        {
            var pot = await _potRepo.GetByIdAsync(id);
            if (pot == null) return null;

            pot.Name = dto.Name;
            pot.TargetAmount = dto.TargetAmount;
            pot.TargetDate = dto.TargetDate;

            await _potRepo.UpdateAsync(pot);
            await _potRepo.SaveChangesAsync();
            return _mapper.Map<SavingsPotDTO>(pot);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pot = await _potRepo.GetByIdAsync(id);
            if (pot == null) return false;
            
            // Return funds to user's main balance (handled elsewhere)
            await _potRepo.DeleteAsync(pot);
            await _potRepo.SaveChangesAsync();
            return true;
        }

        public async Task<SavingsTransactionDto> AddTransactionAsync(SavingsTransactionCreateDto dto)
        {
            var pot = await _potRepo.GetByIdAsync(dto.SavingsPotId);
            if (pot == null) return null;

            var transaction = _mapper.Map<SavingsTransaction>(dto);

            // Update balance
            if (dto.TransactionType == "Deposit")
                pot.CurrentAmount += dto.Amount;
            else if (dto.TransactionType == "Withdraw")
                pot.CurrentAmount -= dto.Amount;

            pot.Transactions.Add(transaction);

            await _transactionRepo.AddAsync(transaction);
            await _potRepo.UpdateAsync(pot);
            await _potRepo.SaveChangesAsync();

            return _mapper.Map<SavingsTransactionDto>(transaction);
        }

        public async Task<List<SavingsTransactionDto>> GetTransactionsByPotIdAsync(int potId)
        {
            var transactions = await _transactionRepo.GetByPotIdAsync(potId);
            return _mapper.Map<List<SavingsTransactionDto>>(transactions);
        }
        public async Task<decimal> GetCurrentBalanceAsync(int id)
        {
            var pot = await _potRepo.GetByIdAsync(id);
            if (pot == null) return 0;

            return pot.CurrentAmount;
        }
    }
}
