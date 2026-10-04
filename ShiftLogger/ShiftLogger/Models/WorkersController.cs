using Microsoft.AspNetCore.Mvc;

namespace ShiftLogger;

[ApiController]
[Route("api/[controller]")]
public class WorkersController : ControllerBase
{
    private readonly ShiftService _service;

    public WorkersController(ShiftService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Worker>>> GetWorkers()
    {
        var workers = await _service.WorkersList();
        return workers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Worker>> GetWorker(int id)
    {
        var worker = await _service.GetWorker(id);

        if (worker == null)
        {
            return NotFound();
        }

        return worker;
    }

    [HttpGet("{id}/Shifts")]
    public async Task<ActionResult<List<Shift>>> GetWorkerShifts(int id)
    {
        var shifts = await _service.GetWorkerShifts(id);

        if (shifts == null)
        {
            return NotFound();
        }

        return shifts;
    }

    [HttpPost]
    public async Task<ActionResult<Worker>> PostWorker(Worker worker)
    {
        await _service.PostWorker(worker);

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

        await _service.PutWorker(worker);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        bool deleted = await _service.DeleteWorker(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}