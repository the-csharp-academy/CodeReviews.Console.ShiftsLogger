using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Data;
using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsDbContext _dbContext;

    public ShiftService(ShiftsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Shift CreateShift(Shift shift)
    {
        var savedShift = _dbContext.Shifts.Add(shift);
        _dbContext.SaveChanges();
        return savedShift.Entity;
    }

    public string? DeleteShift(int id)
    {
        Shift savedShift = _dbContext.Shifts.Find(id);

        if (savedShift == null) return null;

        _dbContext.Shifts.Remove(savedShift);
        _dbContext.SaveChanges();

        return $"Successfully deleted shift with the ID of ${id}";
    }

    public List<Shift> GetAllShifts()
    {
        return _dbContext.Shifts.ToList();
    }

    public Shift? GetShiftById(int id)
    {
        Shift savedShift = _dbContext.Shifts.Find(id);
        return savedShift;
    }

    public Shift UpdateShift(int id, Shift shift)
    {
        Shift savedShift = _dbContext.Shifts.Find(id);

        if (savedShift == null) return null;

        _dbContext.Entry(savedShift).CurrentValues.SetValues(shift);
        _dbContext.SaveChanges();

        return savedShift;
    }
}
