using AutoMapper;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.DTOs.RecurringBill;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System;

namespace PersonalFinanceApplication.Services
{
    public class RecurringBillsService : IRecurringBillService
    {
        private readonly IRecurringBillRepository _repository;
        private readonly IMapper _mapper;

        public RecurringBillsService(IRecurringBillRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RecurringBillDTO> GetRecurringBillByIdAsync(int id)
        {
            var bill = await _repository.GetByIdAsync(id);
            return _mapper.Map<RecurringBillDTO>(bill);
        }

        public async Task<IEnumerable<RecurringBillDTO>> GetUserRecurringBillsAsync(int userId)
        {
            var bills = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RecurringBillDTO>>(bills);
        }

        public async Task<RecurringBillDTO> CreateRecurringBillAsync(CreateRecurringBillDTO dto)
        {
            var bill = _mapper.Map<RecurringBill>(dto);
            var created = await _repository.CreateAsync(bill);
            return _mapper.Map<RecurringBillDTO>(created);
        }

        public async Task<RecurringBillDTO> UpdateRecurringBillAsync(int id, UpdateRecurringBillDTO dto)
        {
            var bill = await _repository.GetByIdAsync(id);
            if (bill == null) return null;

            _mapper.Map(dto, bill);
            var updated = await _repository.UpdateAsync(bill);
            return _mapper.Map<RecurringBillDTO>(updated);
        }

        public async Task<bool> DeleteRecurringBillAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RecurringBillDTO>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var bills = await _repository.GetUpcomingBillsAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<RecurringBillDTO>>(bills);
        }

        public async Task<bool> MarkBillAsPaidAsync(int id)
        {
            var bill = await _repository.GetByIdAsync(id);
            if (bill == null) return false;

            bill.IsPaid = true;
            await _repository.UpdateAsync(bill);
            return true;
        }
    }
}
    