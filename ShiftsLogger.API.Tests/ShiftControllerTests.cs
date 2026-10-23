using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShiftsLogger.API.Tests;

[TestFixture]
public sealed class ShiftControllerTests : ApiControllerTestBase
{
    private ShiftsController _controller = null!;

    [SetUp]
    public void SetUpController()
    {
        _controller = new ShiftsController(new ShiftsService(DbContext));
    }

    [Test]
    public async Task GetById_WhenShiftExists_ReturnsShiftWithCalculatedDurationAndEmployeeDetails()
    {
        var employee = await AddEmployeeAsync();
        employee.FirstName = "Casey";
        employee.LastName = "Taylor";
        employee.Role = "Manager";
        await DbContext.SaveChangesAsync();
        var shift = await AddShiftAsync(employee.Id);

        var result = await _controller.GetById(shift.Id);

        var response = result.Result as OkObjectResult;
        var shiftResponse = response?.Value as ShiftResponse;
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(shiftResponse?.Duration, Is.EqualTo(TimeSpan.FromHours(8)));
            Assert.That(shiftResponse?.Employee, Is.EqualTo(
                new ShiftEmployeeResponse("Casey", "Taylor", "Manager")));
        });
    }

    [Test]
    public async Task GetById_WhenShiftDoesNotExist_ReturnsNotFound()
    {
        var result = await _controller.GetById(999);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Create_WhenEmployeeIsInactiveOrMissing_ReturnsBadRequest(bool employeeExists)
    {
        var employeeId = employeeExists
            ? (await AddEmployeeAsync(isActive: false)).Id
            : 999;

        var result = await _controller.Create(CreateShift(employeeId));

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(await DbContext.Shifts.CountAsync(), Is.Zero);
    }

    [Test]
    public async Task Create_WhenEmployeeIsActive_ReturnsCreatedResponse()
    {
        var employee = await AddEmployeeAsync();

        var result = await _controller.Create(CreateShift(employee.Id));

        var created = result.Result as CreatedAtActionResult;
        var shiftResponse = created?.Value as ShiftResponse;
        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.ActionName, Is.EqualTo(nameof(ShiftsController.GetById)));
            Assert.That(shiftResponse?.Duration, Is.EqualTo(TimeSpan.FromHours(8)));
        });
        Assert.That(await DbContext.Shifts.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task Delete_WhenShiftExists_ReturnsNoContentAndRemovesShift()
    {
        var employee = await AddEmployeeAsync();
        var shift = await AddShiftAsync(employee.Id);

        var result = await _controller.Delete(shift.Id);

        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(await DbContext.Shifts.CountAsync(), Is.Zero);
    }

    [Test]
    public async Task Delete_WhenShiftDoesNotExist_ReturnsNotFound()
    {
        var result = await _controller.Delete(999);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [TestCase(8, 8)]
    [TestCase(9, 8)]
    public void ShiftTimeValidation_RejectsEqualOrReversedTimes(int startHour, int endHour)
    {
        var dto = new CreateShiftDTO
        {
            EmployeeID = 1,
            StartTime = new DateTime(2026, 10, 16, startHour, 0, 0),
            EndTime = new DateTime(2026, 10, 16, endHour, 0, 0)
        };
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            dto,
            new ValidationContext(dto),
            results,
            validateAllProperties: true);

        Assert.That(isValid, Is.False);
        Assert.That(results, Is.Not.Empty);
    }

    [Test]
    public void ShiftTimeValidation_AcceptsEndTimeAfterStartTime()
    {
        var dto = CreateShift(1);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            dto,
            new ValidationContext(dto),
            results,
            validateAllProperties: true);

        Assert.That(isValid, Is.True);
    }

    private static CreateShiftDTO CreateShift(int employeeId)
        => new()
        {
            EmployeeID = employeeId,
            StartTime = new DateTime(2026, 10, 16, 8, 0, 0),
            EndTime = new DateTime(2026, 10, 16, 16, 0, 0)
        };
}
