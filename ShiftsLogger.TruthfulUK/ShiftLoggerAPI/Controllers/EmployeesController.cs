using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Contracts;

namespace ShiftLoggerAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
}
