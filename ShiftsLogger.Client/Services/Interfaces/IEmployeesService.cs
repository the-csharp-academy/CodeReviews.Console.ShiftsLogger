public interface IEmployeesService
{
    Task<IReadOnlyCollection<ClientEmployee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClientEmployee> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(EmployeeWriteRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, EmployeeWriteRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
