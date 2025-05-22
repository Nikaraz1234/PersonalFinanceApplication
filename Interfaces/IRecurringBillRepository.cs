using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IRecurringBillRepository
    {
        Task<RecurringBill> GetByIdAsync(int id);
        Task<IEnumerable<RecurringBill>> GetByUserIdAsync(int userId);
        Task<RecurringBill> CreateAsync(RecurringBill recurringBill);
        Task<RecurringBill> UpdateAsync(RecurringBill recurringBill);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RecurringBill>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate);
    }
}
