using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Services;

namespace PersonalFinanceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;

        public BudgetsController(IBudgetService budgetService, IMapper mapper)
        {
            _budgetService = budgetService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BudgetSummaryDTO>>> GetUserBudgets(int userId)
        {
            var budgets = await _budgetService.GetUserBudgetsAsync(userId);
            return Ok(budgets);
        }

        [HttpGet("{id}/categories")]
        public async Task<ActionResult<IEnumerable<BudgetCategoryDTO>>> GetBudgetCategories(int id)
        {
            var categories = await _budgetService.GetBudgetCategoriesAsync(id);
            return Ok(categories);
        }
  
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDTO>> GetBudget(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null) return NotFound();
            return Ok(budget);
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDTO>> CreateBudget([FromBody] CreateBudgetDTO budgetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBudget = await _budgetService.CreateBudgetAsync(budgetDto);
            return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BudgetDTO>> UpdateBudget(int id, [FromBody] UpdateBudgetDTO budgetDto)
        {
            if (id != budgetDto.Id)
                return BadRequest("ID mismatch");

            try
            {
                var updatedBudget = await _budgetService.UpdateBudgetAsync(id, budgetDto);
                return Ok(updatedBudget);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var result = await _budgetService.DeleteBudgetAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}