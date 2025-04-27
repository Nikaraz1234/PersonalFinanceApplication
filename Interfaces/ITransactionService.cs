using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDTO> GetTransactionAsync(int id);
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync();
        Task<IEnumerable<TransactionDTO>> GetUserTransactionsAsync(int userId);
        Task<IEnumerable<TransactionDTO>> SearchTransactionsAsync(int userId, string searchTerm, DateTime? startDate, DateTime? endDate, int? categoryId);
        Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO transactionDto);
        Task<TransactionDTO> UpdateTransactionAsync(UpdateTransactionDTO transactionDto);
        Task DeleteTransactionAsync(int id);
    }
}
