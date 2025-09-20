using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Entities;

namespace ShiftLoggerAPI.Interfaces;

public interface IEmployeeService
{
    public List<EmployeeResponse> GetAllEmployees();
    public List<Shift> GetAllEmployeeShifts(int id);
    public EmployeeResponse CreateEmployee(CreateEmployeeRequest employee);
    public EmployeeResponse UpdateEmployee(int id, UpdateEmployeeRequest employee);
    public bool DeleteEmployee(int id);
}
