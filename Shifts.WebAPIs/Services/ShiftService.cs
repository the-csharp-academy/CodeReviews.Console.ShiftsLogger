using Microsoft.EntityFrameworkCore;
using Shifts.WebAPIs.Data;
using Shifts.WebAPIs.Models;

namespace Shifts.WebAPIs.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ShiftsDbContext _dbContext;

        public ShiftService(ShiftsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Shift> CreateShift(Shift shift)
        {
            var entity = _dbContext.Shifts.Add(shift);
            await _dbContext.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<bool> DeleteShift(int id)
        {
            var shift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.Id == id);
            if (shift == null)
            {
                return false;
            }
            _dbContext.Remove(shift);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Shift>?> GetAllShifts()
        {
            return await _dbContext.Shifts.ToListAsync();
        }

        public async Task<Shift?> GetShiftById(int id)
        {
            return await _dbContext.Shifts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Shift?> UpdateShift(int id, Shift updatedShift)
        {
            var shift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.Id == id);
            if (shift == null)
            {
                return null;
            }
            shift.StartTime = updatedShift.StartTime;
            shift.EndTime = updatedShift.EndTime;
            shift.Employee = updatedShift.Employee;
            await _dbContext.SaveChangesAsync();
            return shift;
        }
    }
}
