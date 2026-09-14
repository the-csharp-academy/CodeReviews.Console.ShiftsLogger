using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.Api.Dtos;
using ShiftsLogger.Api.Services;

namespace ShiftsLogger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _service;

    public ShiftsController(IShiftService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShiftDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShiftDto>> GetById(int id, CancellationToken ct)
    {
        var shift = await _service.GetByIdAsync(id, ct);
        return shift is null ? NotFound() : Ok(shift);
    }

    [HttpPost]
    public async Task<ActionResult<ShiftDto>> Create(ShiftRequestDto request, CancellationToken ct)
    {
        try
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ShiftValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShiftDto>> Update(int id, ShiftRequestDto request, CancellationToken ct)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, request, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ShiftValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
