using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Interfaces;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryController : ControllerBase
    {
        private readonly IBudgetCategoryService _budgetCategoryService;

        public BudgetCategoryController(IBudgetCategoryService budgetCategoryService)
        {
            _budgetCategoryService = budgetCategoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetCategoryDTO>> GetBudgetCategory(int id)
        {
            var category = await _budgetCategoryService.GetBudgetCategoryAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("budget/{budgetId}")]
        public async Task<ActionResult<IEnumerable<BudgetCategoryDTO>>> GetBudgetCategoriesByBudget(int budgetId)
        {
            var categories = await _budgetCategoryService.GetBudgetCategoriesByBudgetAsync(budgetId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<BudgetCategoryDTO>> CreateBudgetCategory(CreateBudgetCategoryDTO categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdCategory = await _budgetCategoryService.CreateBudgetCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetBudgetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut]
        public async Task<ActionResult<BudgetCategoryDTO>> UpdateBudgetCategory(UpdateBudgetCategoryDTO categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedCategory = await _budgetCategoryService.UpdateBudgetCategoryAsync(categoryDto);
            if (updatedCategory == null) return NotFound();

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetCategory(int id)
        {
            await _budgetCategoryService.DeleteBudgetCategoryAsync(id);
            return NoContent();
        }

        [HttpPatch("{categoryId}/spending")]
        public async Task<IActionResult> UpdateCategorySpending(int categoryId, [FromBody] decimal amount)
        {
            await _budgetCategoryService.UpdateCurrentSpendingAsync(categoryId, amount);
            return NoContent();
        }
    }
}
