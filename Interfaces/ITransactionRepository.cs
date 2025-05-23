﻿using PersonalFinanceApplication.DTOs.Pagination;
using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId);
        Task<IEnumerable<Transaction>> SearchTransactionsAsync(int userId, string searchTerm, DateTime? startDate, DateTime? endDate, int? categoryId, string sortBy, string sortDirection);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
        Task<bool> TransactionExists(int id);
        Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(int categoryId);
        Task<PaginatedResult<Transaction>> GetUserTransactionsPagedAsync(int userId, PaginationParams pagination);
        Task<List<Transaction>> GetLatestThreeTransactionsPerCategoryAsync();
        Task<Dictionary<string, decimal>> GetMonthlySpendingAsync();



    }
}
