using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs.RecurringBill;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringBillController : ControllerBase
    {
        private readonly IRecurringBillService _recurringBillService;
        private readonly IMapper _mapper;

        public RecurringBillController(IRecurringBillService recurringBillService, IMapper mapper)
        {
            _recurringBillService = recurringBillService;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RecurringBillDTO>> GetRecurringBillById(int id)
        {
            var result = await _recurringBillService.GetRecurringBillByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Recurring bill with id {id} not found.");
            }
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<RecurringBillDTO>>> GetUserRecurringBillsAsync(int userId)
        {
            var result = await _recurringBillService.GetUserRecurringBillsAsync(userId);
            if (result == null || !result.Any())
            {
                return NotFound($"No recurring bills found for user with id {userId}.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RecurringBillDTO>> CreateRecurringBillAsync([FromBody] CreateRecurringBillDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var createdBill = await _recurringBillService.CreateRecurringBillAsync(dto);
            return CreatedAtAction(nameof(GetRecurringBillById), new { id = createdBill.Id }, createdBill);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<RecurringBillDTO>> UpdateRecurringBillAsync(int id, [FromBody] UpdateRecurringBillDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedBill = await _recurringBillService.UpdateRecurringBillAsync(id, dto);
            if (updatedBill == null)
            {
                return NotFound($"Recurring bill with id {id} not found.");
            }

            return Ok(updatedBill);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecurringBillAsync(int id)
        {
            var success = await _recurringBillService.DeleteRecurringBillAsync(id);
            if (!success)
            {
                return NotFound($"Recurring bill with id {id} not found.");
            }
            return NoContent();
        }

        [HttpGet("upcoming/{userId}")]
        public async Task<ActionResult<IEnumerable<RecurringBillDTO>>> GetUpcomingBillsAsync(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _recurringBillService.GetUpcomingBillsAsync(userId, startDate, endDate);
            if (result == null || !result.Any())
            {
                return NotFound($"No upcoming bills found for user with id {userId} between {startDate} and {endDate}.");
            }
            return Ok(result);
        }

        [HttpPatch("markpaid/{id}")]
        public async Task<ActionResult> MarkBillAsPaidAsync(int id)
        {
            var success = await _recurringBillService.MarkBillAsPaidAsync(id);
            if (!success)
            {
                return NotFound($"Recurring bill with id {id} not found.");
            }

            return NoContent();
        }
    }
}
