using PersonalFinanceApplication.DTOs.RecurringBill;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IRecurringBillService
    {
        Task<RecurringBillDTO> GetRecurringBillByIdAsync(int id);
        Task<IEnumerable<RecurringBillDTO>> GetUserRecurringBillsAsync(int userId);
        Task<RecurringBillDTO> CreateRecurringBillAsync(CreateRecurringBillDTO dto);
        Task<RecurringBillDTO> UpdateRecurringBillAsync(int id, UpdateRecurringBillDTO dto);
        Task<bool> DeleteRecurringBillAsync(int id);
        Task<IEnumerable<RecurringBillDTO>> GetUpcomingBillsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<bool> MarkBillAsPaidAsync(int id);
    }
}
