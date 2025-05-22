using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceApplication.Repositories
{
    public class RecurringBillRepository : IRecurringBillRepository
    {
        private readonly AppDbContext _context;

        public RecurringBillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RecurringBill> GetByIdAsync(int id)
        {
            return await _context.RecurringBills.FindAsync(id);
        }

        public async Task<IEnumerable<RecurringBill>> GetByUserIdAsync(int userId)
        {
            return await _context.RecurringBills.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<RecurringBill> CreateAsync(RecurringBill recurringBill)
        {
            _context.RecurringBills.Add(recurringBill);
            await _context.SaveChangesAsync();
            return recurringBill;
        }

        public async Task<RecurringBill> UpdateAsync(RecurringBill recurringBill)
        {
            _context.RecurringBills.Update(recurringBill);
            await _context.SaveChangesAsync();
            return recurringBill;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bill = await _context.RecurringBills.FindAsync(id);
            if (bill == null) return false;

            _context.RecurringBills.Remove(bill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RecurringBill>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.RecurringBills
                .Where(b =>
                b.UserId == userId &&
                IsBillDueBetweenDates(b.DueDay, startDate, endDate))
                .ToListAsync();
        }
        private bool IsBillDueBetweenDates(int dueDay, DateTime start, DateTime end)
        {
            for (var dt = start.Date; dt <= end.Date; dt = dt.AddDays(1))
            {
                if (dt.Day == dueDay)
                    return true;
            }
            return false;
        }

    }
}