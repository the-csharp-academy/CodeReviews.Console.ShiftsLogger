using Microsoft.EntityFrameworkCore;
using Shifts_Logger.Data;
using Shifts_Logger.Models;

namespace Shifts_Logger.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsDbContext _context;

    public ShiftService(ShiftsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Shift>> GetAllShifts()
    {
        var shifts = await _context.Shifts.ToListAsync();

        return shifts;
    }

    public async Task<Shift?> GetShiftById(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
        {
            return null;
        }

        return shift;
    }

    public async Task<Shift> CreateShift(Shift shift)
    {
        var savedShift = await _context.Shifts.AddAsync(shift);

        await _context.SaveChangesAsync();

        return savedShift.Entity;
    }

    public async Task<Shift> UpdateShift(int id, Shift updatedShift)
    {
        var existingShift = await _context.Shifts.FindAsync(id);

        if (existingShift == null)
        {
            throw new KeyNotFoundException($"Shift with id {id} not found.");
        }

        existingShift.EmployeeName = updatedShift.EmployeeName;
        existingShift.Position = updatedShift.Position;
        existingShift.ShiftNumber = updatedShift.ShiftNumber;
        existingShift.StartTime = updatedShift.StartTime;
        existingShift.EndTime = updatedShift.EndTime;

        _context.Shifts.Update(existingShift);
        await _context.SaveChangesAsync();

        return existingShift;
    }

    public async Task<string?> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
        {
            return $"Shift with id {id} not found.";
        }

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();

        return $"Shift with id {id} successfully deleted."; // Indicating successful deletion
    }
}
