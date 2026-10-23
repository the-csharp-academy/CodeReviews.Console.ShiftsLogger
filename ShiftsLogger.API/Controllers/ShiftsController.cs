using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftsService _shiftsService;

    public ShiftsController(IShiftsService shiftsService)
    {
        _shiftsService = shiftsService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<ShiftResponse>>> GetAll()
    {
        var shifts = await _shiftsService.GetAllAsync();
        return Ok(shifts.Select(ToResponse).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShiftResponse>> GetById(int id)
    {
        var shift = await _shiftsService.GetByIdAsync(id);
        return shift is null ? NotFound() : Ok(ToResponse(shift));
    }

    [HttpPost]
    public async Task<ActionResult<ShiftResponse>> Create(CreateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var createdShift = await _shiftsService.CreateAsync(shift);
        if (createdShift is null)
            return BadRequest("Cannot assign shift: the employee does not exist or is inactive.");

        var response = ToResponse(createdShift);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShiftResponse>> Update(int id, UpdateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var updateResult = await _shiftsService.UpdateAsync(id, shift);
        return updateResult.Status switch
        {
            ShiftUpdateStatus.Updated when updateResult.Shift is not null
                => Ok(ToResponse(updateResult.Shift)),
            ShiftUpdateStatus.ShiftNotFound => NotFound(),
            ShiftUpdateStatus.InvalidEmployeeAssignment
                => BadRequest("The shift cannot be reassigned to an employee that does not exist or is inactive."),
            _ => throw new InvalidOperationException("The shift update returned an invalid result.")
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _shiftsService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private static ShiftResponse ToResponse(Shift shift)
        => new(
            shift.Id,
            shift.EmployeeId,
            shift.StartTime,
            shift.EndTime,
            new ShiftEmployeeResponse(
                shift.Employee.FirstName,
                shift.Employee.LastName,
                shift.Employee.Role));
}
