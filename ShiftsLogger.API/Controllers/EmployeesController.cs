using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Employee>>> GetAll()
        => Ok(await _employeeService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create(CreateEmployeeDTO dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
        };

        var createdEmployee = await _employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Employee>> Update(int id, EmployeeDTO dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
        };

        var updatedEmployee = await _employeeService.UpdateAsync(id, employee, dto.IsActive);
        return updatedEmployee is null ? NotFound() : Ok(updatedEmployee);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/archive")]
    public async Task<ActionResult<Employee>> Archive(int id)
    {
        var employee = await _employeeService.ArchiveAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPatch("{id:int}/restore")]
    public async Task<ActionResult<Employee>> Restore(int id)
    {
        var employee = await _employeeService.RestoreAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }
}
