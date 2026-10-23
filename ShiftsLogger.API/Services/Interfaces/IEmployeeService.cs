public interface IEmployeeService
{
    Task<ICollection<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee dto);
    Task<Employee?> UpdateAsync(int id, Employee dto, bool? isActive = null);
    Task<bool> DeleteAsync(int id);
    Task<Employee?> ArchiveAsync(int id);
    Task<Employee?> RestoreAsync(int id);
}