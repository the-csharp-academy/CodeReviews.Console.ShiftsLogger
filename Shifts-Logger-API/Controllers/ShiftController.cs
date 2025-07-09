using Microsoft.AspNetCore.Mvc;
using Shifts_Logger.Models;
using Shifts_Logger.Services;

namespace Shifts_Logger.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<Shift?>> GetShiftById(int id)
    {
        var shift = await _shiftService.GetShiftById(id);

        if (shift == null)
            return NotFound();

        return Ok(shift);
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> CreateShift(Shift shift)
    {
        var createdShift = await _shiftService.CreateShift(shift);

        return new ObjectResult(createdShift) { StatusCode = 201 };
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Shift>> UpdateShift(int id, Shift updatedShift)
    {
        var shift = await _shiftService.UpdateShift(id, updatedShift);

        if (shift == null)
            return NotFound();

        return Ok(shift);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string?>> DeleteShift(int id)
    {
        var result = await _shiftService.DeleteShift(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
