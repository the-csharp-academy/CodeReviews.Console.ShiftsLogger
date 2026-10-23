using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShiftsLogger.API.Tests;

[TestFixture]
public sealed class EmployeeControllerTests : ApiControllerTestBase
{
    private EmployeesController _controller = null!;

    [SetUp]
    public void SetUpController()
    {
        _controller = new EmployeesController(new EmployeeService(DbContext));
    }

    [Test]
    public async Task GetAll_ReturnsEmployees()
    {
        await AddEmployeeAsync();

        var result = await _controller.GetAll();

        var response = result.Result as OkObjectResult;
        Assert.That(response?.Value, Is.InstanceOf<ICollection<Employee>>());
        Assert.That((ICollection<Employee>)response!.Value!, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetById_WhenEmployeeExists_ReturnsEmployee()
    {
        var employee = await AddEmployeeAsync();

        var result = await _controller.GetById(employee.Id);

        var response = result.Result as OkObjectResult;
        Assert.That(response?.Value, Is.SameAs(employee));
    }

    [Test]
    public async Task GetById_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        var result = await _controller.GetById(999);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtGetByIdAndPersistsEmployee()
    {
        var dto = new CreateEmployeeDTO
        {
            FirstName = "Casey",
            LastName = "Taylor",
            Role = "Manager"
        };

        var result = await _controller.Create(dto);

        var created = result.Result as CreatedAtActionResult;
        var employee = created?.Value as Employee;
        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.ActionName, Is.EqualTo(nameof(EmployeesController.GetById)));
            Assert.That(employee?.FirstName, Is.EqualTo("Casey"));
            Assert.That(employee?.isActive, Is.True);
        });
        Assert.That(await DbContext.Employees.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task Update_WhenEmployeeExists_UpdatesFieldsAndActiveStatus()
    {
        var employee = await AddEmployeeAsync();
        var dto = new EmployeeDTO
        {
            FirstName = "Casey",
            LastName = "Taylor",
            Role = "Manager",
            IsActive = false
        };

        var result = await _controller.Update(employee.Id, dto);

        var response = result.Result as OkObjectResult;
        var updatedEmployee = response?.Value as Employee;
        Assert.Multiple(() =>
        {
            Assert.That(updatedEmployee?.FirstName, Is.EqualTo("Casey"));
            Assert.That(updatedEmployee?.LastName, Is.EqualTo("Taylor"));
            Assert.That(updatedEmployee?.Role, Is.EqualTo("Manager"));
            Assert.That(updatedEmployee?.isActive, Is.False);
        });
    }

    [Test]
    public async Task Update_WhenActiveStatusIsOmitted_PreservesCurrentStatus()
    {
        var employee = await AddEmployeeAsync(isActive: false);
        var dto = new EmployeeDTO
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Role = employee.Role
        };

        var result = await _controller.Update(employee.Id, dto);

        Assert.That((result.Result as OkObjectResult)?.Value, Is.SameAs(employee));
        Assert.That(employee.isActive, Is.False);
    }

    [Test]
    public async Task Update_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        var result = await _controller.Update(999, new EmployeeDTO());

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task ArchiveAndRestore_ToggleEmployeeActiveStatus()
    {
        var employee = await AddEmployeeAsync();

        var archiveResult = await _controller.Archive(employee.Id);
        var restoreResult = await _controller.Restore(employee.Id);

        Assert.Multiple(() =>
        {
            Assert.That(archiveResult.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(restoreResult.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(employee.isActive, Is.True);
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task ArchiveOrRestore_WhenEmployeeDoesNotExist_ReturnsNotFound(bool archive)
    {
        var result = archive
            ? await _controller.Archive(999)
            : await _controller.Restore(999);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_WhenEmployeeExists_ReturnsNoContentAndRemovesEmployee()
    {
        var employee = await AddEmployeeAsync();

        var result = await _controller.Delete(employee.Id);

        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(await DbContext.Employees.CountAsync(), Is.Zero);
    }

    [Test]
    public async Task Delete_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        var result = await _controller.Delete(999);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
