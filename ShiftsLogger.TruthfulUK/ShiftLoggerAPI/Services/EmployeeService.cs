using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Data;
using ShiftLoggerAPI.Entities;
using ShiftLoggerAPI.Contracts.Employees;
using ShiftLoggerAPI.Contracts.Shifts;
using Microsoft.EntityFrameworkCore;
using ShiftLoggerAPI.Mapping;

namespace ShiftLoggerAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ShiftsDbContext _dbContext;

    public EmployeeService(ShiftsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public EmployeeResponse CreateEmployee(CreateEmployeeRequest employee)
    {
        var newEmployee = new Employee(employee.Name, employee.Title);

        _dbContext.Employees.Add(newEmployee);
        _dbContext.SaveChanges();

        return newEmployee.ToResponse();
    }

    public bool DeleteEmployee(int id)
    {
        var employee = _dbContext.Employees.Find(id);

        if (employee == null) return false;

        _dbContext.Employees.Remove(employee);
        _dbContext.SaveChanges();

        return true;
    }

    public List<EmployeeResponse> GetAllEmployees()
    {
        return _dbContext.Employees
            .AsNoTracking()
            .Select (e => e.ToResponse())
            .ToList();
    }

    public EmployeeResponse? GetEmployeeById(int id)
    {
        var employee = _dbContext.Employees
            .AsNoTracking()
            .SingleOrDefault(e => e.Id == id);

        if (employee == null) return null;
        
        return employee.ToResponse();
    }

    public List<ShiftResponse>? GetAllEmployeeShifts(int employeeId)
    {
        var employeeExists = _dbContext.Employees
            .AsNoTracking()
            .Any(e => e.Id == employeeId);

        if (!employeeExists) return null;

        return _dbContext.Shifts.Where(e => e.EmployeeId == employeeId)
            .AsNoTracking()
            .Select(s => s.ToResponse())
            .ToList();
    }

    public EmployeeResponse? UpdateEmployee(int id, UpdateEmployeeRequest employee)
    {
        var targetEmployee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
        
        if (targetEmployee == null) return null;

        targetEmployee.Update(employee.Name, employee.Title);

        _dbContext.SaveChanges();

        return targetEmployee.ToResponse();
    }
}
