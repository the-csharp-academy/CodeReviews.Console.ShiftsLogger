using ShiftsLogger.UI.Models;

namespace ShiftsLogger.UI.Services;

public interface IShiftApiClient
{
    Task<List<ShiftDto>> GetAllAsync(CancellationToken ct = default);
    Task<ShiftDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ShiftDto> CreateAsync(ShiftRequest request, CancellationToken ct = default);
    Task<ShiftDto?> UpdateAsync(int id, ShiftRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
