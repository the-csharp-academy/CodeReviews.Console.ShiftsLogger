using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Contracts.Shifts;

namespace ShiftLoggerAPI.Controllers;
[ApiController]
[Route("api/shifts")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftsController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpGet]
    public ActionResult<List<ShiftResponse>> GetAllShifts()
    {
        var allShifts = _shiftService.GetAllShifts();

        if (allShifts == null) return NotFound();

        return Ok(allShifts);
    }

    [HttpGet("{id}")]
    public ActionResult<ShiftResponse> GetShiftById(int id)
    {
        var shift = _shiftService.GetShiftById(id);

        if (shift == null) return NotFound();

        return Ok(shift);
    }

    [HttpPost]
    public ActionResult<ShiftResponse> CreateShift(CreateShiftRequest shift)
    {
        var newShift = _shiftService.CreateShift(shift);

        return (newShift != null)
            ? CreatedAtAction(
                nameof(GetShiftById),
                new { id = newShift.Id }, newShift)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID",
                detail: $"There is no employee with the ID {shift.EmployeeId}.");
    }

    [HttpPut("{id}")]
    public ActionResult<ShiftResponse> UpdateShift(int id, UpdateShiftRequest shift)
    {
        var updatedShift = _shiftService.UpdateShift(id, shift);

        return (updatedShift != null)
            ? CreatedAtAction(
                nameof(GetShiftById),
                new { id = updatedShift.Id }, updatedShift)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID or Shift ID",
                detail: $"The updated shift contains an invalid Employee ID or Shift ID.");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteShift(int id)
    {
        var deleted = _shiftService.DeleteShift(id);

        if (!deleted) return NotFound();

        return NoContent();
    }
}
