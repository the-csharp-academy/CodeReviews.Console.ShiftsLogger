using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Controllers;
[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public ActionResult<List<Employee>> GetAllEmployees()
    {
        return Ok(_employeeService.GetAllEmployees());
    }

    [HttpGet("{EmployeeId}/shifts")]
    public ActionResult<List<Shift>> GetAllEmployeeShifts(int EmployeeId)
    {
        return Ok(_employeeService.GetAllEmployeeShifts(EmployeeId));
    }

    [HttpPost]
    public ActionResult<Employee> CreateEmployee(Employee employee)
    {
        return Ok(_employeeService.CreateEmployee(employee));
    }

    [HttpPut]
    public ActionResult<Employee> UpdateEmployee(int id, Employee updatedEmployee)
    {
        return Ok(_employeeService.UpdateEmployee(id, updatedEmployee));
    }

    [HttpDelete]
    public ActionResult<string> DeleteEmployee(int id)
    {
        return Ok(_employeeService.DeleteEmployee(id));
    }
}
