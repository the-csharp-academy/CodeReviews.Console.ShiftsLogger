using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShiftLogger;

[ApiController]
[Route("api/[Controller]")]
public class ShiftsController : ControllerBase
{
    private readonly ShiftLoggerContext _context;
    private readonly ShiftService _service;

    public ShiftsController(ShiftLoggerContext context)
    {
        _context = context;
        
        _service = new ShiftService(_context);
    }

    [HttpGet]
    public async Task <ActionResult<List<Shift>>> GetShifts()
    {
        var shifts = await _context.Shifts
            .ToListAsync();

        if (shifts == null || shifts.Count == 0)
        {
            return NotFound();
        }

        return shifts;
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<Shift>> GetShift(int id)
    {
        var shift = await _context.Shifts
            .FirstOrDefaultAsync(s => s.Id == id);

        if(shift == null)
        {
            return NotFound();
        }
        return shift;
    }

    [HttpPost]
    public async Task <ActionResult<Shift>> PostShift(Shift shift)
    {
        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);

        if(!valid)
            return BadRequest();

        bool workerExists = await _service.WorkerExists(shift.WorkerId);

        if(!workerExists)
            return BadRequest();

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShift), new {id = shift.Id}, shift);
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> PutShift(int id, Shift shift)
    {
        if(id != shift.Id)
        {
            return BadRequest();
        }

        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);

        if(!valid)
            return BadRequest();

        bool workerExists = await _service.WorkerExists(shift.WorkerId);

        if(!workerExists)
            return BadRequest();

        _context.Entry(shift).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task <IActionResult> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if(shift == null)
        {
            return NotFound();
        }

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}