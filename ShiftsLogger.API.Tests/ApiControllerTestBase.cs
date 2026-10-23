using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ShiftsLogger.API.Tests;

public abstract class ApiControllerTestBase
{
    private SqliteConnection _connection = null!;
    protected ShiftsLoggerDbContext DbContext { get; private set; } = null!;

    [SetUp]
    public async Task SetUpDatabaseAsync()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ShiftsLoggerDbContext>()
            .UseSqlite(_connection)
            .Options;
        DbContext = new ShiftsLoggerDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDownDatabaseAsync()
    {
        await DbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }

    protected async Task<Employee> AddEmployeeAsync(bool isActive = true)
    {
        var employee = new Employee
        {
            FirstName = "Test",
            LastName = "Employee",
            Role = "Tester",
            isActive = isActive
        };

        DbContext.Employees.Add(employee);
        await DbContext.SaveChangesAsync();
        return employee;
    }

    protected async Task<Shift> AddShiftAsync(int employeeId)
    {
        var shift = new Shift
        {
            EmployeeId = employeeId,
            StartTime = new DateTime(2026, 10, 16, 8, 0, 0),
            EndTime = new DateTime(2026, 10, 16, 16, 0, 0)
        };

        DbContext.Shifts.Add(shift);
        await DbContext.SaveChangesAsync();
        return shift;
    }
}
