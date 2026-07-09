using Microsoft.AspNetCore.Mvc;
using silvermax.shiftlogger.Dtos;
using silvermax.shiftlogger.Models;
using silvermax.shiftlogger.Services;

namespace silvermax.shiftlogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftService _service;

    public ShiftController(IShiftService shiftService)
    {
        _service = shiftService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Shift>>> GetAllShifts()
    {
        var shifts = await _service.GetAllShiftsAsync(CancellationToken.None);
        return Ok(shifts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Shift>> GetShiftById(Guid id)
    {
        var result = await _service.GetShiftByIdAsync(id, CancellationToken.None);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> CreateShift(CreateShiftDto dto)
    {
        var shift = new Shift
        {
            Name = dto.Name,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var result = await _service.CreateShiftAsync(shift, CancellationToken.None);

        return CreatedAtAction(nameof(GetShiftById), new { id = result.ShiftId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Shift>> UpdateShift(Guid id, Shift updatedShift)
    {
        var result = await _service.UpdateShiftAsync(id, updatedShift, CancellationToken.None);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<string>> DeleteShift(Guid id)
    {
        var result = await _service.DeleteshiftAsync(id, CancellationToken.None);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
