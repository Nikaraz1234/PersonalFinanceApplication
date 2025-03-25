using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IRecurringBillService
    {
        Task<RecurringBill> GetRecurringBillByIdAsync(int id);
        Task<IEnumerable<RecurringBill>> GetUserRecurringBillsAsync(int userId);
        Task<RecurringBill> CreateRecurringBillAsync(RecurringBill recurringBill);
        Task<RecurringBill> UpdateRecurringBillAsync(RecurringBill recurringBill);
        Task<bool> DeleteRecurringBillAsync(int id);
        Task<IEnumerable<RecurringBill>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<bool> MarkBillAsPaidAsync(int id);
    }
}
