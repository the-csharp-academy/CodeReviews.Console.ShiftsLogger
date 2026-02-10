using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTOs.Worker;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;
    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkerDto>> AddWorker(WorkerCreationDto workerDto)
    {
        var worker = await _workerService.AddWorkerAsync(workerDto);
        if (worker is null)
        {
            return BadRequest("Could not create worker.");
        }
        return CreatedAtAction(nameof(GetWorkerById), new { id = worker.Id }, worker);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkerDto>> GetWorkerById(int id)
    {
        var worker = await _workerService.GetWorkerByIdAsync(id);
        if (worker is null)
        { 
            return NotFound();
        }

        return Ok(worker);
    }

    [HttpGet("{id}/Details")]
    public async Task<ActionResult<WorkerWithShiftsDto>> GetWorkerWithShiftsById(int id)
    {
        var worker = await _workerService.GetWorkerWithShiftsAsync(id);
        if (worker is null)
        { 
            return NotFound();
        }

        return Ok(worker);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<WorkerDto>>> GetAllWorkers()
    {
        return Ok(await _workerService.GetAllWorkersAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Worker>> UpdateWorker(int id, WorkerCreationDto updateWorker)
    {
        var worker = await _workerService.GetWorkerByIdAsync(id);
        if (worker is null)
        {
            return NotFound();
        }
        var updatedWorker = await _workerService.UpdateWorkerAsync(id, updateWorker);
        if (updatedWorker is null)
        {
            return BadRequest("Can't update the worker!");
        }
        return Ok(updatedWorker);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorker(int id)
    {
        bool isWorkerDeleted = await _workerService.DeleteWorkerAsync(id);

        if (isWorkerDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }


}
