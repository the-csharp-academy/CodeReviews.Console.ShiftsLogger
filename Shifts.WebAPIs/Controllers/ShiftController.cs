using Microsoft.AspNetCore.Mvc;
using Shifts.WebAPIs.Models;
using Shifts.WebAPIs.Services;

namespace Shifts.WebAPIs.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Shift>>> GetAllShifts()
        {
            return Ok(await _shiftService.GetAllShifts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShiftById(int id)
        {
            var result = await _shiftService.GetShiftById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Shift>> CreateShift(Shift shift)
        {
            if (shift.EndTime < shift.StartTime)
            {
                return BadRequest(new { error = $"EndTime: {shift.EndTime} must be greater then StartTime: {shift.StartTime}." });
            }
            return Ok(await _shiftService.CreateShift(shift));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Shift>> UpdateShift(int id, Shift updatedShift)
        {
            var result = await _shiftService.UpdateShift(id, updatedShift);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteShift(int id)
        {
            var result = await _shiftService.DeleteShift(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
