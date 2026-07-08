using Azure.Core;
using Microsoft.EntityFrameworkCore;
using silvermax.shiftlogger.Data;
using silvermax.shiftlogger.Models;

namespace silvermax.shiftlogger.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftDbContext _db;

    public ShiftService(ShiftDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<Shift> CreateShiftAsync(Shift shift, CancellationToken ct)
    {
        var newShift = _db.Shifts.Add(shift);
        await _db.SaveChangesAsync(ct);

        return newShift.Entity;
    }

    public async Task<string> DeleteshiftAsync(Guid id, CancellationToken ct)
    {
        var shiftToDelete = await _db.Shifts.FirstOrDefaultAsync(s => s.ShiftId == id, ct);

        if (shiftToDelete is null)
            return null;

        _db.Shifts.Remove(shiftToDelete);
        await _db.SaveChangesAsync(ct);

        return $"Shift with Id {id} deleted successfully";
    }

    public async Task<List<Shift>> GetAllShiftsAsync(CancellationToken ct) => await _db.Shifts.ToListAsync(ct);

    public async Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken ct)
    {
        var shift = await _db.Shifts.FindAsync(id, ct);

        return shift;
    }

    public async Task<Shift> UpdateShiftAsync(Guid id, Shift updatedShift, CancellationToken ct)
    {
        var shiftToUpdate = await _db.Shifts.FindAsync(id, ct);

        if (shiftToUpdate is null)
            return null;

        _db.Entry(shiftToUpdate).CurrentValues.SetValues(updatedShift);
        await _db.SaveChangesAsync(ct);

        return shiftToUpdate;
    }
}
