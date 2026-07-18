using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.DTOs;
using ShiftsLogger.Interfaces;
using ShiftsLogger.Model;

namespace ShiftsLogger.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class WorkerShiftController(IWorkerShiftService workerShiftService) : Controller
{
    private readonly IWorkerShiftService _workerShiftService = workerShiftService;


    [HttpGet("All")]
    public ActionResult<List<WorkerShift>> GetAllWorkerShifts()
    {
        return Ok(_workerShiftService.GetWorkerShifts());
    }

    [HttpGet("Get={id}")]
    public ActionResult<WorkerShift> GetWorkerShiftById(int id)
    {
        var result = _workerShiftService.GetWorkerShitftById(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("Create")]
    public ActionResult<WorkerShift> CreateWorkerShift(CreatedWorkerShiftDTO dto)
    {
        WorkerShift workerShift = _workerShiftService.CreateWorkerShift(dto);

        return CreatedAtAction(nameof(GetWorkerShiftById), new { id = workerShift.ShiftId}, workerShift);
    }

    [HttpPatch("Update={id}")]
    public ActionResult<WorkerShift> UpdateWorkerShitft(int id, [FromBody] UpdateWorkerShiftDto dto)
    {
        WorkerShift? workerShift = _workerShiftService.UpdateWorkerShift(id, dto);

        if(workerShift == null) 
        {
            return NotFound();
        }

        return Ok(workerShift);
    }

    [HttpDelete("Delete={id}")]
    public IActionResult DeleteWorkerShift(int id)
    {
        bool result = _workerShiftService.DeleteWorkerShift(id);

        if (!result) return NotFound();

        return NoContent();
    }
}