using ShiftLoggerAPI.Contracts.Employees;
using ShiftLoggerAPI.Contracts.Shifts;

namespace ShiftLoggerAPI.Interfaces;

public interface IEmployeeService
{
    public List<EmployeeResponse> GetAllEmployees();
    public EmployeeResponse? GetEmployeeById(int id);
    public List<ShiftResponse>? GetAllEmployeeShifts(int id);
    public EmployeeResponse CreateEmployee(CreateEmployeeRequest employee);
    public EmployeeResponse? UpdateEmployee(int id, UpdateEmployeeRequest employee);
    public bool DeleteEmployee(int id);
}
