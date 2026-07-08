using silvermax.shiftlogger.Models;
using System.ComponentModel;

namespace silvermax.shiftlogger.Services;

public interface IShiftService
{
    public Task<List<Shift>> GetAllShiftsAsync(CancellationToken ct);
    public Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken ct);
    public Task<Shift> CreateShiftAsync(Shift shift, CancellationToken ct);
    public Task<Shift> UpdateShiftAsync(Guid id, Shift updatedShift, CancellationToken ct);
    public Task<string> DeleteshiftAsync(Guid id, CancellationToken ct);
}
