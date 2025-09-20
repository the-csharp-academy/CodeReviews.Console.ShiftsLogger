using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Interfaces;

public interface IEmployeeService
{
    public List<Employee> GetAllEmployees();
    public List<Shift> GetAllEmployeeShifts(int id);
    public Employee CreateEmployee(Employee employee);
    public Employee UpdateEmployee(int id, Employee employee);
    public string? DeleteEmployee(int id);
}
