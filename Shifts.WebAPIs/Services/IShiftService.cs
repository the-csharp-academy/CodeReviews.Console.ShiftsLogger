using Shifts.WebAPIs.Models;

namespace Shifts.WebAPIs.Services
{
    public interface IShiftService
    {
        public Task<Shift?> GetShiftById(int id);
        public Task<List<Shift>?> GetAllShifts();
        public Task<Shift> CreateShift(Shift shift);
        public Task<Shift?> UpdateShift(int id, Shift shift);
        public Task<bool> DeleteShift(int id);
    }
}
