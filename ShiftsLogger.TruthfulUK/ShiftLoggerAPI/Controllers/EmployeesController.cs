using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Contracts.Employees;
using ShiftLoggerAPI.Contracts.Shifts;
using ShiftLoggerAPI.Interfaces;

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

        return (allEmployees != null)
            ? Ok(allEmployees)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "No Employees",
                detail: "There is currently no employees in the database.");
    }

    [HttpGet("{id:int}")]
    public ActionResult<EmployeeResponse> GetEmployeeById(int id)
    {
        var employee = _employeeService.GetEmployeeById(id);

        return (employee != null)
            ? Ok(employee)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID",
                detail: $"There is no employee with the ID {id}.");
    }

    [HttpGet("{id:int}/shifts")]
    public ActionResult<List<ShiftResponse>> GetAllEmployeeShifts(int id)
    {
        var employeeShifts = _employeeService.GetAllEmployeeShifts(id);

        return (employeeShifts != null)
            ? Ok(employeeShifts)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID",
                detail: $"There is no employee with the ID {id}.");
    }

    [HttpPost]
    public ActionResult<EmployeeResponse> CreateEmployee(CreateEmployeeRequest employee)
    {
        var newEmployee = _employeeService.CreateEmployee(employee);

        return (newEmployee != null)
            ? CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = newEmployee.Id }, newEmployee)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Error.",
                detail: "Unknown error processing employee creation.");
    }

    [HttpPut("{id:int}")]
    public ActionResult<EmployeeResponse> UpdateEmployee(int id, UpdateEmployeeRequest updatedEmployeeDetails)
    {
        var updatedEmployee = _employeeService.UpdateEmployee(id, updatedEmployeeDetails);

        return (updatedEmployee != null)
            ? CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = updatedEmployee.Id }, updatedEmployee)
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID",
                detail: $"There is no employee with the ID {id}.");
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteEmployee(int id)
    {
        var isDeleted = _employeeService.DeleteEmployee(id);

        return (isDeleted) 
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Invalid Employee ID",
                detail: $"There is no employee with the ID {id}.");
    }
}
