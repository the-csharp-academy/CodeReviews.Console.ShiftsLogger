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

    [Fact]
    public async Task CreateAsync_Throws_WhenEndIsBeforeStart()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        var bad = new ShiftRequestDto("Ana",
            new DateTime(2026, 9, 14, 16, 0, 0),
            new DateTime(2026, 9, 14, 8, 0, 0));

        var ex = await Assert.ThrowsAsync<ShiftValidationException>(() => service.CreateAsync(bad));
        Assert.Equal("End time must be after the start time.", ex.Message);
        Assert.Equal(0, await db.Shifts.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenEndEqualsStart()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        var same = new ShiftRequestDto("Ana",
            new DateTime(2026, 9, 14, 8, 0, 0),
            new DateTime(2026, 9, 14, 8, 0, 0));

        await Assert.ThrowsAsync<ShiftValidationException>(() => service.CreateAsync(same));
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenWorkerNameIsWhitespace()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        await Assert.ThrowsAsync<ShiftValidationException>(() => service.CreateAsync(Valid("   ")));
    }

    [Fact]
    public async Task CreateAsync_TrimsWorkerName()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        var created = await service.CreateAsync(Valid("  Ana  "));

        Assert.Equal("Ana", created.WorkerName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsShiftsOrderedByStartTime()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        await service.CreateAsync(new ShiftRequestDto("Later",
            new DateTime(2026, 9, 15, 9, 0, 0), new DateTime(2026, 9, 15, 17, 0, 0)));
        await service.CreateAsync(new ShiftRequestDto("Earlier",
            new DateTime(2026, 9, 14, 9, 0, 0), new DateTime(2026, 9, 14, 17, 0, 0)));

        var all = await service.GetAllAsync();

        Assert.Equal(new[] { "Earlier", "Later" }, all.Select(s => s.WorkerName).ToArray());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        Assert.Null(await service.GetByIdAsync(999));
    }

    [Fact]
    public async Task UpdateAsync_ChangesStoredShift()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);
        var created = await service.CreateAsync(Valid());

        var updated = await service.UpdateAsync(created.Id, new ShiftRequestDto("Bea",
            new DateTime(2026, 9, 14, 10, 0, 0), new DateTime(2026, 9, 14, 18, 0, 0)));

        Assert.NotNull(updated);
        Assert.Equal("Bea", updated!.WorkerName);
        Assert.Equal(new DateTime(2026, 9, 14, 18, 0, 0), updated.EndTime);
        Assert.Equal(1, await db.Shifts.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenMissing()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        Assert.Null(await service.UpdateAsync(999, Valid()));
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenEndIsBeforeStart()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);
        var created = await service.CreateAsync(Valid());

        var bad = new ShiftRequestDto("Ana",
            new DateTime(2026, 9, 14, 16, 0, 0), new DateTime(2026, 9, 14, 8, 0, 0));

        await Assert.ThrowsAsync<ShiftValidationException>(() => service.UpdateAsync(created.Id, bad));
    }

    [Fact]
    public async Task DeleteAsync_RemovesShift_And_ReturnsTrue()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);
        var created = await service.CreateAsync(Valid());

        Assert.True(await service.DeleteAsync(created.Id));
        Assert.Equal(0, await db.Shifts.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenMissing()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        Assert.False(await service.DeleteAsync(999));
    }

    [Fact]
    public async Task ShiftDuration_IsComputedFromStartAndEnd()
    {
        using var db = CreateContext();
        var service = new ShiftService(db);

        var created = await service.CreateAsync(Valid());
        var stored = await db.Shifts.AsNoTracking().FirstAsync(s => s.Id == created.Id);

        Assert.Equal(TimeSpan.FromHours(8.5), stored.Duration);
    }
}
