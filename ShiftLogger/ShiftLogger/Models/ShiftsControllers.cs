using Microsoft.AspNetCore.Mvc;

namespace ShiftLogger;

[ApiController]
[Route("api/[Controller]")]
public class ShiftsController : ControllerBase
{
    private readonly ShiftService _service;

    public ShiftsController(ShiftService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Shift>>> GetShifts()
    {
        var shifts = await _service.ShiftsList();

        return shifts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> GetShift(int id)
    {
        var shift = await _service.GetShift(id);

        if (shift == null)
        {
            return NotFound();
        }

        return shift;
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> PostShift(Shift shift)
    {
        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);

        if (!valid)
            return BadRequest();

        bool workerExists = await _service.WorkerExists(shift.WorkerId);

        if (!workerExists)
            return BadRequest();

        await _service.PostShift(shift);

        return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shift);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutShift(int id, Shift shift)
    {
        if (id != shift.Id)
        {
            return BadRequest();
        }

        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);

        if (!valid)
            return BadRequest();

        bool workerExists = await _service.WorkerExists(shift.WorkerId);

        if (!workerExists)
            return BadRequest();

        await _service.PutShift(shift);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        bool deleted = await _service.DeleteShift(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}