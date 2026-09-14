using ShiftsLogger.Api.Dtos;

namespace ShiftsLogger.Api.Services;

public interface IShiftService
{
    Task<IReadOnlyList<ShiftDto>> GetAllAsync(CancellationToken ct = default);
    Task<ShiftDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ShiftDto> CreateAsync(ShiftRequestDto request, CancellationToken ct = default);
    Task<ShiftDto?> UpdateAsync(int id, ShiftRequestDto request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
