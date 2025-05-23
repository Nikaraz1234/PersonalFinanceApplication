﻿using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs.Pagination;
using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.Interfaces;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet("user/{userId}/paged")]
        public async Task<ActionResult<PaginatedResult<TransactionDTO>>> GetUserTransactionsPaged(
        int userId,
        [FromQuery] PaginationParams pagination)
        {
            var pagedTransactions = await _transactionService.GetUserTransactionsPagedAsync(userId, pagination);
            return Ok(pagedTransactions);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> SearchTransactions(
            [FromQuery] int userId,
            [FromQuery] string searchTerm = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string sortBy = "Date",
            [FromQuery] string sortDirection = "desc")
        {
            var transactions = await _transactionService.SearchTransactionsAsync(
                userId, searchTerm, startDate, endDate, categoryId, sortBy, sortDirection);

            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction(CreateTransactionDTO transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
        }
        [HttpGet("latest-three-per-category")]
        public async Task<IActionResult> GetLatestThreeTransactionsPerCategory()
        {
            var result = await _transactionService.GetLatestThreeTransactionsPerCategoryAsync();
            return Ok(result);
        }

        [HttpGet("monthly-spending")]
        public async Task<IActionResult> GetMonthlySpending()
        {
            var result = await _transactionService.GetMonthlySpendingAsync();
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<TransactionDTO>> UpdateTransaction(UpdateTransactionDTO transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTransaction = await _transactionService.UpdateTransactionAsync(transactionDto);
            if (updatedTransaction == null)
            {
                return NotFound();
            }

            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
