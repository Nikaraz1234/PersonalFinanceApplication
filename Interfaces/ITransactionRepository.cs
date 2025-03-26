using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetTransactionsAsync(int userId, int page = 1, int pageSize = 10);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(int id, TransactionDTO transactionDto);
        Task DeleteTransactionAsync(int id);

    }
}
