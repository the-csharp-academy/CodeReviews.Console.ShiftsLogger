using Shifts_Logger.Models;

namespace Shifts_Logger.Services;

public interface IShiftService
{
    public Task<List<Shift>> GetAllShifts();

    public Task<Shift?> GetShiftById(int id);

    public Task<Shift> CreateShift(Shift shift);

    public Task<Shift> UpdateShift(int id, Shift updatedShift);

    public Task<string?> DeleteShift(int id);
}
