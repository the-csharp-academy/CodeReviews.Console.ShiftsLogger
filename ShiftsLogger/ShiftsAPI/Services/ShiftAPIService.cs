using ShiftsAPI.Data;
using ClassLibray.Models;

namespace ShiftsAPI.Services;

public interface IShiftAPIService
{
    public Shift CreateShift(Shift shift);
    public List<Shift> GetAllShifts();
    public Shift? GetShiftById(int id);
    public Shift? UpdateShift(int id, Shift updatedShift);
    public string DeleteShift(int id);
}
public class ShiftAPIService : IShiftAPIService
{
    private readonly ShiftContext _dbContext;
    public ShiftAPIService(ShiftContext DbContext)
    {
        _dbContext = DbContext;
    }

    public Shift CreateShift(Shift shift)
    {
        var createdShift = _dbContext.Shifts.Add(shift);
        _dbContext.SaveChanges();
        return createdShift.Entity;
    }

    public List<Shift> GetAllShifts()
    {
        return _dbContext.Shifts.ToList();
    }

    public Shift? GetShiftById(int id)
    {
        return _dbContext.Shifts.Find(id);
    }

    public Shift? UpdateShift(int id, Shift updatedShift)
    {
        var targetShift = _dbContext.Shifts.Find(id);
        if (targetShift == null) return null;

        _dbContext.Entry(targetShift).CurrentValues.SetValues(updatedShift);
        _dbContext.SaveChanges();
        return targetShift;
    }

    public string DeleteShift(int id)
    {
        var targetShift = _dbContext.Shifts.Find(id);
        if (targetShift == null) return null;

        _dbContext.Shifts.Remove(targetShift);
        _dbContext.SaveChanges();
        return $"Shift with id {id} has been deleted successfully";
    }
}