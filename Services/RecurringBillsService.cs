using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System;

namespace PersonalFinanceApplication.Services
{
    public class RecurringBillsService : IRecurringBillService
    {
        private readonly AppDbContext _context;

        public RecurringBillsService(AppDbContext context)
        {
            _context = context;
        }

        public Task<RecurringBill> CreateRecurringBillAsync(RecurringBill recurringBill)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRecurringBillAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RecurringBill> GetRecurringBillByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecurringBill>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecurringBill>> GetUserRecurringBillsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkBillAsPaidAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RecurringBill> UpdateRecurringBillAsync(RecurringBill recurringBill)
        {
            throw new NotImplementedException();
        }
    }
}
    