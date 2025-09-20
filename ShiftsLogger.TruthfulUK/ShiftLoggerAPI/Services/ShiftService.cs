using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Data;
using ShiftLoggerAPI.Entities;
using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ShiftLoggerAPI.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsDbContext _dbContext;

    public ShiftService(ShiftsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ShiftResponse CreateShift(CreateShiftRequest shift)
    {
        var newShift = new Shift(shift.Start, shift.End, shift.EmployeeId);
        _dbContext.Shifts.Add(newShift);
        _dbContext.SaveChanges();
        return newShift.ToResponse();
    }

    public bool DeleteShift(int id)
    {
        var shift = _dbContext.Shifts.Find(id);

        if (shift == null) return false;

        _dbContext.Shifts.Remove(shift);
        _dbContext.SaveChanges();

        return true;
    }

    public List<ShiftResponse> GetAllShifts()
    {
        return _dbContext.Shifts
            .AsNoTracking()
            .Select(s => s.ToResponse())
            .ToList();
    }

    public ShiftResponse? GetShiftById(int id)
    {
        var shift = _dbContext.Shifts.Find(id);

        if (shift == null) return null;

        return shift.ToResponse();
    }

    public ShiftResponse? UpdateShift(int id, UpdateShiftRequest shift)
    {
        var targetShift = _dbContext.Shifts.FirstOrDefault(x => x.Id == id);

        if (targetShift == null) return null;

        targetShift.Update(shift.Start, shift.End, shift.EmployeeId);
        
        _dbContext.SaveChanges();

        return targetShift.ToResponse();
    }
}
