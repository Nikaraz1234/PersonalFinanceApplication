using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.DTOs;


namespace PersonalFinanceApplication.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if(transaction == null)
            {
                throw new Exception($"Transaction with id {id} cannot be found");
            }
            _context.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task UpdateTransactionAsync(int id,TransactionDTO transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if(transaction == null)
            {
                throw new Exception($"Transaction with id {id} cannot be found");
            }
            transaction.Description = transactionDto.Description;
            transaction.Category = transactionDto.Category;
            transaction.Amount = transactionDto.Amount;
            transaction.Date = transaction.Date;
            await _context.SaveChangesAsync();
        }
    }
}
