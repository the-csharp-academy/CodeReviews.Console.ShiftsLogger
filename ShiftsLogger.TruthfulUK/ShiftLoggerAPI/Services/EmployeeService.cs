using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Data;
using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ShiftsDbContext _dbContext;

    public EmployeeService(ShiftsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Employee CreateEmployee(Employee employee)
    {
        var savedEmployee = _dbContext.Employees.Add(employee);
        _dbContext.SaveChanges();
        return savedEmployee.Entity;
    }

    public string? DeleteEmployee(int id)
    {
        Employee savedEmployee = _dbContext.Employees.Find(id);

        if (savedEmployee == null) return null;

        _dbContext.Employees.Remove(savedEmployee);
        _dbContext.SaveChanges();

        return $"Successfully deleted the employee with ID of {id}";
    }

    public List<Employee> GetAllEmployees()
    {
        return _dbContext.Employees.ToList();
    }

    public Employee UpdateEmployee(int id, Employee employee)
    {
        Employee savedEmployee = _dbContext.Employees.Find(id);

        if (savedEmployee == null) return null;

        _dbContext.Entry(savedEmployee).CurrentValues.SetValues(employee);
        _dbContext.SaveChanges();

        return savedEmployee;
    }
}
