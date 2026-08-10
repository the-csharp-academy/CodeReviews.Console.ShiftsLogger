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
    public async Task<ActionResult<List<Shift>>> GetAllShifts(CancellationToken ct)
    {
        var shifts = await _service.GetAllShiftsAsync(ct);
        return Ok(shifts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Shift>> GetShiftById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetShiftByIdAsync(id, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> CreateShift(CreateShiftDto dto, CancellationToken ct)
    {
        var shift = new Shift
        {
            Name = dto.Name,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var result = await _service.CreateShiftAsync(shift, ct);

        return CreatedAtAction(nameof(GetShiftById), new { id = result.ShiftId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Shift>> UpdateShift(Guid id, UpdateShiftDto dto, CancellationToken ct)
    {

        var result = await _service.UpdateShiftAsync(id, dto, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<string>> DeleteShift(Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteshiftAsync(id, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
