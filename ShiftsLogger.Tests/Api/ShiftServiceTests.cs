using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Data;
using ShiftsLogger.Api.Dtos;
using ShiftsLogger.Api.Services;

namespace ShiftsLogger.Tests.Api;

public class ShiftServiceTests
{
    private static ShiftsDbContext CreateContext()
    {
        // An open :memory: connection keeps the schema alive for the lifetime of the context.
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<ShiftsDbContext>()
            .UseSqlite(connection)
            .Options;
        var db = new ShiftsDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static ShiftRequestDto Valid(string worker = "Ana") => new(
        worker,
        new DateTime(2026, 9, 14, 8, 0, 0),
        new DateTime(2026, 9, 14, 16, 30, 0));

    [Fact]
    public async Task CreateAsync_PersistsShift_And_ReturnsDto()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        var created = await service.CreateAsync(Valid());

        Assert.True(created.Id > 0);
        Assert.Equal("Ana", created.WorkerName);
        Assert.Equal(new DateTime(2026, 9, 14, 8, 0, 0), created.StartTime);
        Assert.Equal(1, await db.Shifts.CountAsync());
    }
}
