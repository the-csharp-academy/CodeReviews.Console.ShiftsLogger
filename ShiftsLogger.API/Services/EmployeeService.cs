
using Microsoft.EntityFrameworkCore;

public class EmployeeService : IEmployeeService
{
    private readonly ShiftsLoggerDbContext _shiftsCtx;

    public EmployeeService(ShiftsLoggerDbContext ctx)
    {
        _shiftsCtx = ctx;
    }

    public async Task<Employee?> ArchiveAsync(int id)
        => await SetActiveStatusAsync(id, false);

    public async Task<Employee> CreateAsync(Employee dto)
    {
        var saved = _shiftsCtx.Employees.Add(dto);
        await _shiftsCtx.SaveChangesAsync();
        return saved.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var saved = await _shiftsCtx.Employees.FindAsync(id);
        if (saved == null)
            return false;

        _shiftsCtx.Employees.Remove(saved);
        await _shiftsCtx.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Employee>> GetAllAsync()
        => await _shiftsCtx.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await _shiftsCtx.Employees.FindAsync(id);

    public async Task<Employee?> RestoreAsync(int id)
        => await SetActiveStatusAsync(id, true);

    public async Task<Employee?> UpdateAsync(int id, Employee dto, bool? isActive = null)
    {
        var saved = await _shiftsCtx.Employees.FindAsync(id);
        if (saved == null)
            return null;

        // _shiftsCtx.Entry(saved).CurrentValues.SetValues(dto);
        saved.FirstName = dto.FirstName;
        saved.LastName = dto.LastName;
        saved.Role = dto.Role;
        if (isActive.HasValue)
            saved.isActive = isActive.Value;
        await _shiftsCtx.SaveChangesAsync();
        return saved;
    }

    private async Task<Employee?> SetActiveStatusAsync(int id, bool isActive)
    {
        var employee = await GetByIdAsync(id);

        if (employee == null)
            return null;

        employee.isActive = isActive;
        await _shiftsCtx.SaveChangesAsync();
        return employee;
    }
}