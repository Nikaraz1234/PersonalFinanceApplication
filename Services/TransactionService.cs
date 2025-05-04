using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Repositories;
using PersonalFinanceApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using PersonalFinanceApplication.DTOs.Transaction;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.DTOs.Pagination;

namespace PersonalFinanceApplication.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.Date = DateTime.UtcNow;

            var createdTransaction = await _transactionRepository.CreateTransactionAsync(transaction);
            return _mapper.Map<TransactionDTO>(createdTransaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO> GetTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionAsync(id);
            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<IEnumerable<TransactionDTO>> GetUserTransactionsAsync(int userId)
        {
            var transactions = await _transactionRepository.GetUserTransactionsAsync(userId);
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<TransactionDTO>> SearchTransactionsAsync(int userId, string searchTerm, DateTime? startDate, DateTime? endDate, int? categoryId,
        string sortBy = "Date",
        string sortDirection = "desc")
        {
            var transactions = await _transactionRepository.SearchTransactionsAsync(userId, searchTerm, startDate, endDate, categoryId, sortBy, sortDirection);
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO> UpdateTransactionAsync(UpdateTransactionDTO transactionDto)
        {
            var existingTransaction = await _transactionRepository.GetTransactionAsync(transactionDto.Id);
            if (existingTransaction == null)
            {
                return null;
            }

            _mapper.Map(transactionDto, existingTransaction);

            var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(existingTransaction);
            return _mapper.Map<TransactionDTO>(updatedTransaction);
        }
        public async Task<PaginatedResult<TransactionDTO>> GetUserTransactionsPagedAsync(int userId, PaginationParams pagination)
        {
            var result = await _transactionRepository.GetUserTransactionsPagedAsync(userId, pagination);

            return new PaginatedResult<TransactionDTO>
            {
                Items = result.Items.Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    Date = t.Date,
                    CategoryName = t.BudgetCategory?.Name,
                }),
                TotalCount = result.TotalCount
            };
        }

    }
}
