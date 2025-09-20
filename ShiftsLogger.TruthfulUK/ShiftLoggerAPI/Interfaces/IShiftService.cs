using ShiftLoggerAPI.Entities;

namespace ShiftLoggerAPI.Interfaces;

public interface IShiftService
{
    public List<Shift> GetAllShifts();
    public Shift? GetShiftById(int id);
    public Shift CreateShift(Shift shift);
    public Shift UpdateShift(int id, Shift shift);
    public string? DeleteShift(int id);
}
