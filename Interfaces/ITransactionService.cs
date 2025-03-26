using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId, int page = 1, int pageSize = 10);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(int id, TransactionDTO transactionDto);
        Task<bool> DeleteTransactionAsync(int id);
    }
}
