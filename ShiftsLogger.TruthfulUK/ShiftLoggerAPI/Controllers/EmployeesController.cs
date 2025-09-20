using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Entities;
using ShiftLoggerAPI.Contracts;

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
    public ActionResult<List<EmployeeResponse>> GetAllEmployees()
    {
        var allEmployees = _employeeService.GetAllEmployees();

        if (allEmployees == null)
        {
            return NotFound();
        }

        return Ok(allEmployees);
    }

    [HttpGet("{id:int}/shifts")]
    public ActionResult<List<Shift>> GetAllEmployeeShifts(int id)
    {
        var employeeShifts = _employeeService.GetAllEmployeeShifts(id);

        if (employeeShifts == null)
        {
            return NotFound();
        }

        return Ok(employeeShifts);
    }

    [HttpPost]
    public ActionResult<EmployeeResponse> CreateEmployee(CreateEmployeeRequest employee)
    {
        var dto = _employeeService.CreateEmployee(employee);
        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<EmployeeResponse> UpdateEmployee(int id, UpdateEmployeeRequest updatedEmployee)
    {
        var dto = _employeeService.UpdateEmployee(id, updatedEmployee);
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteEmployee(int id)
    {
        var deleted = _employeeService.DeleteEmployee(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
