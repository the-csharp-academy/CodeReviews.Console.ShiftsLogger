using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Contracts;

public interface IEmployeeService
{
    public List<Employee> GetAllEmployees();
    public Employee CreateEmployee(Employee employee);
    public Employee UpdateEmployee(int id, Employee employee);
    public string? DeleteEmployee(int id);
}
