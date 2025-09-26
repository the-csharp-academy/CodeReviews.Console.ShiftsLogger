using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Contracts.Shifts;
using ShiftLoggerAPI.Interfaces;

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

        return (allShifts != null)
            ? Ok(allShifts)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "No Employees",
                detail: "There is currently no employees in the database.");
    }

    [HttpGet("{id}")]
    public ActionResult<ShiftResponse> GetShiftById(int id)
    {
        var shift = _shiftService.GetShiftById(id);

        return (shift != null)
            ? Ok(shift)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Shift ID",
                detail: $"There is no shift with the ID {id}.");
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
        var isDeleted = _shiftService.DeleteShift(id);

        return (isDeleted)
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Shift ID",
                detail: $"There is no shift with the ID {id}.");
    }
}
