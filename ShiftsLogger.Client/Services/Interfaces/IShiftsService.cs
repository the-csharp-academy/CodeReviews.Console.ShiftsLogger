public interface IShiftsService
{
    Task<IReadOnlyCollection<ClientShift>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClientShift> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(ShiftWriteRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ShiftWriteRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
