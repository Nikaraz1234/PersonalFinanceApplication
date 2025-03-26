using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Repositories;
using PersonalFinanceApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PersonalFinanceApplication.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionRepository _repo;

        public TransactionService(TransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await _repo.AddTransactionAsync(transaction);
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            await _repo.DeleteTransactionAsync(id);
            return true;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _repo.GetTransactionsAsync(userId);
        }
            

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _repo.GetTransactionByIdAsync(id);
        }

        public async Task<Transaction> UpdateTransactionAsync(int id, TransactionDTO transactionDto)
        {
            await _repo.UpdateTransactionAsync(id, transactionDto);
            var transaction = await _repo.GetTransactionByIdAsync(id);
            return transaction;
        }
    }
}
