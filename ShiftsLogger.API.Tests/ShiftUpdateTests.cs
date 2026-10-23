using Microsoft.AspNetCore.Mvc;

namespace ShiftsLogger.API.Tests;

[TestFixture]
public sealed class ShiftUpdateTests : ApiControllerTestBase
{
    private ShiftsService _shiftsService = null!;
    private ShiftsController _controller = null!;

    [SetUp]
    public void SetUpServices()
    {
        _shiftsService = new ShiftsService(DbContext);
        _controller = new ShiftsController(_shiftsService);
    }

    [Test]
    public async Task Update_WithUnchangedInactiveEmployee_DoesNotRejectExistingAssignment()
    {
        var employee = await AddEmployeeAsync(isActive: false);
        var shift = await AddShiftAsync(employee.Id);

        var result = await _controller.Update(shift.Id, new UpdateShiftDTO
        {
            EmployeeID = employee.Id,
            StartTime = shift.StartTime.AddHours(1),
            EndTime = shift.EndTime.AddHours(1)
        });

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        Assert.That(shift.StartTime, Is.EqualTo(new DateTime(2026, 10, 16, 9, 0, 0)));
    }

    [Test]
    public async Task Update_WithInvalidReassignment_ReturnsBadRequest()
    {
        var assignedEmployee = await AddEmployeeAsync(isActive: true);
        var inactiveEmployee = await AddEmployeeAsync(isActive: false);
        var shift = await AddShiftAsync(assignedEmployee.Id);

        var result = await _controller.Update(shift.Id, new UpdateShiftDTO
        {
            EmployeeID = inactiveEmployee.Id,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime
        });

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(shift.EmployeeId, Is.EqualTo(assignedEmployee.Id));
    }

    [Test]
    public async Task Update_WhenShiftDoesNotExist_ReturnsNotFound()
    {
        var inactiveEmployee = await AddEmployeeAsync(isActive: false);

        var result = await _controller.Update(999, new UpdateShiftDTO
        {
            EmployeeID = inactiveEmployee.Id,
            StartTime = new DateTime(2026, 10, 16, 8, 0, 0),
            EndTime = new DateTime(2026, 10, 16, 16, 0, 0)
        });

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAll_IncludesEmployeeDetailsInShiftResponse()
    {
        var employee = await AddEmployeeAsync(isActive: true);
        employee.FirstName = "Casey";
        employee.LastName = "Taylor";
        employee.Role = "Manager";
        await DbContext.SaveChangesAsync();
        await AddShiftAsync(employee.Id);

        var result = await _controller.GetAll();
        var response = result.Result as OkObjectResult;
        var shifts = response?.Value as ICollection<ShiftResponse>;

        Assert.That(shifts, Has.Count.EqualTo(1));
        Assert.That(shifts!.Single().Employee, Is.EqualTo(
            new ShiftEmployeeResponse("Casey", "Taylor", "Manager")));
    }
}
