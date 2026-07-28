using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Data;
using ShiftsLogger.DTOs;
using ShiftsLogger.Models;

namespace ShiftsLogger.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ShiftsLoggerContext _context;
        private readonly IMapper _mapper;

        public ShiftService(ShiftsLoggerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        public async Task<List<ShiftReadDTO>> GetAllShiftsAsync()
        {
            return await _context.Shifts
                .AsNoTracking()
                .ProjectTo<ShiftReadDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<ShiftReadDTO>> GetAllShiftsByWorkerAsync(int workerId)
        {
            return await _context.Shifts
                .AsNoTracking()
                .Where(s => s.WorkerId == workerId)
                .ProjectTo<ShiftReadDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ShiftReadDTO?> GetShiftByIdAsync(int id)
        {
            return await _context.Shifts
                .AsNoTracking()
                .Where(s => s.Id == id)
                .ProjectTo<ShiftReadDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<ShiftReadDTO> CreateShiftAsync(ShiftCreateDTO dto)
        {
            var shift = _mapper.Map<Shift>(dto);

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            var created = await _context.Shifts
                .Include(s => s.Worker)
                .FirstAsync(s => s.Id == shift.Id);

            return _mapper.Map<ShiftReadDTO>(created);
        }

        public async Task<ShiftReadDTO?> UpdateShiftAsync(int id, ShiftCreateDTO dto)
        {
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);

            if (shift == null)
            {
                return null;
            }

            _mapper.Map(dto, shift);
            await _context.SaveChangesAsync();

            var updated = await _context.Shifts
                .Include(s => s.Worker)
                .FirstAsync(s => s.Id == shift.Id);
            return _mapper.Map<ShiftReadDTO>(updated);
        }

        public async Task<bool> DeleteShiftAsync(int id)
        {
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);

            if (shift == null)
            {
                return false;
            }
            
            _context.Remove(shift);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
