using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Budget>>> GetUserBudgets(int userId)
        {
            var budgets = await _budgetService.GetUserBudgetsAsync(userId);
            return Ok(budgets);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            return Ok(budget);
        }


        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget([FromBody] Budget budget)
        {
            var createdBudget = await _budgetService.CreateBudgetAsync(budget);
            return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] BudgetDTO budgetDto)
        {
            if (id != budgetDto.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            var updatedBudget = await _budgetService.UpdateBudgetAsync(id, budgetDto);
            return Ok(updatedBudget);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var result = await _budgetService.DeleteBudgetAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}