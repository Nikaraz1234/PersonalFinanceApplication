using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs.SavingsPot;
using PersonalFinanceApplication.Interfaces;

namespace PersonalFinanceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingsPotController : ControllerBase
    {
        private readonly ISavingsPotService _savingsPotService;

        public SavingsPotController(ISavingsPotService savingsPotService)
        {
            _savingsPotService = savingsPotService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var pots = await _savingsPotService.GetAllByUserAsync(userId);
            return Ok(pots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pot = await _savingsPotService.GetByIdAsync(id);
            if (pot == null) return NotFound();
            return Ok(pot);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSavingsPotDTO dto)
        {
            var result = await _savingsPotService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSavingsPotDTO dto)
        {
            var updated = await _savingsPotService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _savingsPotService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> AddTransaction([FromBody] SavingsTransactionCreateDto dto)
        {
            var result = await _savingsPotService.AddTransactionAsync(dto);
            if (result == null) return NotFound("Savings pot not found.");
            return Ok(result);
        }

        [HttpGet("transactions/{potId}")]
        public async Task<IActionResult> GetTransactionsByPotId(int potId)
        {
            var transactions = await _savingsPotService.GetTransactionsByPotIdAsync(potId);
            return Ok(transactions);
        }
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetCurrentBalance(int id)
        {
            var balance = await _savingsPotService.GetCurrentBalanceAsync(id);
            if (balance == 0) return NotFound("Savings Pot not found");

            return Ok(balance);
        }
    }
}
