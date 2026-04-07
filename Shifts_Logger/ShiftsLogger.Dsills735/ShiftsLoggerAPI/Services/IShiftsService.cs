using ShiftsLoggerAPI.Data;
using ShiftsLoggerAPI.Models;

namespace ShiftsLoggerAPI.Services
{
    public interface IShiftsService
    {
        public List<Shift> GetAllShifts();
        public Shift? GetShiftByID(int id);
        public Shift CreateShift(Shift shift);

        public Shift UpdateShift(int id, Shift shift);

        public string? DeleteShift(int id);
    }

    public class ShiftService : IShiftsService
    {
        private readonly ShiftsDbContext _context;
        public ShiftService(ShiftsDbContext context)
        {
            _context = context;
        }
        public List<Shift> GetAllShifts()
        {
            return _context.Shifts?.ToList() ?? new List<Shift>();
        }
        public Shift? GetShiftByID(int id)
        {
            return _context.Shifts?.Find(id);
        }
        public Shift CreateShift(Shift shift)
        {
            _context.Shifts!.Add(shift);
            _context.SaveChanges();
            return shift;
        }
        public Shift UpdateShift(int id, Shift shift)
        {
            var existingShift = _context.Shifts?.Find(id);
            if (existingShift == null)
            {
                throw new Exception("Shift not found");
            }
            existingShift.start = shift.start;
            existingShift.end = shift.end;
            existingShift.JobTitle = shift.JobTitle;
            _context.SaveChanges();
            return existingShift;
        }
        public string? DeleteShift(int id)
        {
            var shift = _context.Shifts?.Find(id);
            if (shift == null)
            {
                return "Shift not found";
            }
            _context.Shifts?.Remove(shift);
            _context.SaveChanges();
            return "Shift deleted successfully";
        }
    }
}
