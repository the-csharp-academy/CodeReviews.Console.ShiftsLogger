using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTOs.shift;

namespace ShiftsLogger.API.Services;

public interface IShiftService
{
    Task<GetShiftDto?> AddShiftAsync(CreateShiftDto shiftDto);
    Task<GetShiftDto?> GetShiftByIdAsync(int id);
    //public Task<List<Shift>> GetAllShiftsAsync();
    Task<GetShiftDto?> UpdateShiftAsync(int id, UpdateShiftDto updatedShift);
    Task<bool> RemoveShift(int id);
    Task<List<ShiftDto>?> GetShiftsByWorkerIdAsync(int id);
}
public class ShiftService : IShiftService
{
    private readonly AppDbContext _context;
    public ShiftService(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<GetShiftDto?> AddShiftAsync(CreateShiftDto shiftDto)
    {
        var worker = await _context.Workers.FindAsync(shiftDto.WorkerId);
        if (worker is null)
            return null;
        
        var newShift = new Shift
        {
            StartTime = shiftDto.StartTime,
            EndTime = shiftDto.EndTime,
            WorkerId = shiftDto.WorkerId
        };
        _context.Shifts.Add(newShift);
        await _context.SaveChangesAsync();
        var shift = await GetShiftByIdAsync(newShift.Id);
        return shift;
    }

    public async Task<GetShiftDto?> GetShiftByIdAsync(int id)
    {
        var shift = await _context.Shifts.Include(s => s.Worker).FirstOrDefaultAsync(s => s.Id == id);
        if (shift is null)
        {
            return null;
        }
        var result = new GetShiftDto
        {
            Id = shift.Id,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            WorkerId = shift.WorkerId,
            WorkerName = shift.Worker!.Name
        };
        return result;

    } 
    public async Task<List<ShiftDto>?> GetShiftsByWorkerIdAsync(int id)
    {
        var shifts = _context.Shifts.Where(s => s.WorkerId == id);
        var result = await shifts.Select(s => new ShiftDto
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToListAsync();
        return result;
    }

    public async Task<GetShiftDto?> UpdateShiftAsync(int id, UpdateShiftDto updatedShift)
    {
        var existShift = await _context.Shifts.FindAsync(id);
        if (existShift is null)
        {
            return null;
        }
        existShift.StartTime = updatedShift.StartTime;
        existShift.EndTime = updatedShift.EndTime;
        await _context.SaveChangesAsync();

        var result = await GetShiftByIdAsync(id);
        return result;
    }
    public async Task<bool> RemoveShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift is null) return false;

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }
}
