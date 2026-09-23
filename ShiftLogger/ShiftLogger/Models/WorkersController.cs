using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShiftLogger;

[ApiController]
[Route("api/[controller]")]
public class WorkersController : ControllerBase
{
    private readonly ShiftLoggerContext _context;

    private readonly ShiftService _service;

    public WorkersController(ShiftLoggerContext context)
    {
        _context = context;

        _service = new ShiftService(_context);
    }

    [HttpGet]
    public async Task<ActionResult<List<Worker>>> GetWorkers()
    {
        return await _context.Workers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Worker>> GetWorker(int id)
    {
        var worker = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
        {
            return NotFound();
        }

        return worker;
    }

    [HttpGet("{id}/Shifts")]
    public async Task<ActionResult<List<Shift>>> GetWorkerShifts(int id)
    {
        var shifts = await _context.Shifts
            .Where(s => s.WorkerId == id)
            .ToListAsync();

        if (shifts == null || shifts.Count == 0)
        {
            return NotFound();
        }

        return shifts;
    }

    [HttpPost]
    public async Task<ActionResult<Worker>> PostWorker(Worker worker)
    {
        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorker), new { id = worker.Id }, worker);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutWorker(int id, Worker worker)
    {
        if (id != worker.Id)
        {
            return BadRequest();
        }

        bool workerExists = await _service.WorkerExists(worker.Id);

        if (!workerExists)
            return BadRequest();

        _context.Entry(worker).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker == null)
        {
            return NotFound();
        }

        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}