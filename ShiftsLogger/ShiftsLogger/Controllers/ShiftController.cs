using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.DTOs;
using ShiftsLogger.Services;

namespace ShiftsLogger.Controllers;

[ApiController]
[Route("api/shifts")]
public class ShiftController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShiftsAsync()
    {
        var result = await _shiftService.GetAllShiftsAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShiftAsync(int id)
    {
        var result = await _shiftService.GetShiftByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShiftAsync(ShiftCreateDTO dto)
    {
        var result = await _shiftService.CreateShiftAsync(dto);

        if (result == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetShift", new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShiftAsync(int id, ShiftCreateDTO dto)
    {
        var result = await _shiftService.UpdateShiftAsync(id, dto);

        if (result == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShiftAsync(int id)
    {
        var result = await _shiftService.DeleteShiftAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
