using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Data;
using ShiftsLogger.Api.Dtos;
using ShiftsLogger.Api.Models;

namespace ShiftsLogger.Api.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsDbContext _db;

    public ShiftService(ShiftsDbContext db) => _db = db;

    public async Task<IReadOnlyList<ShiftDto>> GetAllAsync(CancellationToken ct = default)
    {
        var shifts = await _db.Shifts
            .AsNoTracking()
            .OrderBy(s => s.StartTime)
            .ToListAsync(ct);

        return shifts.Select(ToDto).ToList();
    }

    public async Task<ShiftDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var shift = await _db.Shifts.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        return shift is null ? null : ToDto(shift);
    }

    public async Task<ShiftDto> CreateAsync(ShiftRequestDto request, CancellationToken ct = default)
    {
        Validate(request);

        var shift = new Shift
        {
            WorkerName = request.WorkerName.Trim(),
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        _db.Shifts.Add(shift);
        await _db.SaveChangesAsync(ct);
        return ToDto(shift);
    }

    public async Task<ShiftDto?> UpdateAsync(int id, ShiftRequestDto request, CancellationToken ct = default)
    {
        Validate(request);

        var shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (shift is null)
        {
            return null;
        }

        shift.WorkerName = request.WorkerName.Trim();
        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;

        await _db.SaveChangesAsync(ct);
        return ToDto(shift);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (shift is null)
        {
            return false;
        }

        _db.Shifts.Remove(shift);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static void Validate(ShiftRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.WorkerName))
        {
            throw new ShiftValidationException("Worker name is required.");
        }

        if (request.EndTime <= request.StartTime)
        {
            throw new ShiftValidationException("End time must be after the start time.");
        }
    }

    private static ShiftDto ToDto(Shift shift) =>
        new(shift.Id, shift.WorkerName, shift.StartTime, shift.EndTime);
}
