using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Entities;

namespace ShiftLoggerAPI.Interfaces;

public interface IShiftService
{
    public List<ShiftResponse> GetAllShifts();
    public ShiftResponse? GetShiftById(int id);
    public ShiftResponse CreateShift(CreateShiftRequest shift);
    public ShiftResponse? UpdateShift(int id, UpdateShiftRequest shift);
    public bool DeleteShift(int id);
}
