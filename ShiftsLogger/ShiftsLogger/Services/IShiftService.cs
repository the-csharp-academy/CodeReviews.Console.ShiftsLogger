using ShiftsLogger.DTOs;

namespace ShiftsLogger.Services
{
    public interface IShiftService
    {
        public Task<List<ShiftReadDTO>> GetAllShiftsAsync();
        public Task<List<ShiftReadDTO>> GetAllShiftsByWorkerAsync(int workerId);
        public Task<ShiftReadDTO?> GetShiftByIdAsync(int id);
        public Task<ShiftReadDTO> CreateShiftAsync(ShiftCreateDTO dto);
        public Task<ShiftReadDTO?> UpdateShiftAsync(int id, ShiftCreateDTO dto);
        public Task<bool> DeleteShiftAsync(int id);
    }
}
