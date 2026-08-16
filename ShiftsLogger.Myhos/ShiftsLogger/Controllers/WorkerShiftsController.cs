using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.DTOs;
using ShiftsLogger.Interfaces;
using ShiftsLogger.Model;

namespace ShiftsLogger.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class WorkerShiftsController(IWorkerShiftService workerShiftService) : Controller
{
    private readonly IWorkerShiftService _workerShiftService = workerShiftService;


    [HttpGet]
    public ActionResult<List<Shift>> GetAllWorkerShifts()
    {
        return Ok(_workerShiftService.GetWorkerShifts());
    }

    [HttpGet("{id}")]
    public ActionResult<Shift> GetWorkerShiftById(int id)
    {
        return  Ok(_workerShiftService.GetWorkerShitftById(id));
    }

    [HttpPost]
    public ActionResult<Shift> CreateWorkerShift(CreatedWorkerShiftDTO dto)
    {
        Shift workerShift = _workerShiftService.CreateWorkerShift(dto);

        return CreatedAtAction(nameof(GetWorkerShiftById), new { id = workerShift.ShiftId}, workerShift);
    }

    [HttpPatch("{id}")]
    public ActionResult<Shift> UpdateWorkerShitft(int id, [FromBody] UpdateWorkerShiftDto dto)
    {
        Shift? workerShift = _workerShiftService.UpdateWorkerShift(id, dto);
        return Ok(workerShift);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWorkerShift(int id)
    {
        _workerShiftService.DeleteWorkerShift(id);
        return NoContent();
    }
}