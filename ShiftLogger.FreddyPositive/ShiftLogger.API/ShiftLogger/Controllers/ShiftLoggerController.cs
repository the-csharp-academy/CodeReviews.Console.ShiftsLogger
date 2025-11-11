using Microsoft.AspNetCore.Mvc;
using ShiftLogger.Service;
using static ShiftLogger.Models.ShiftLoggerDto.ShiftLoggerDto;

namespace ShiftLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftLoggerController : ControllerBase
{
    private readonly IShiftLoggerService _shiftLoggerService;

    public ShiftLoggerController(IShiftLoggerService shiftLoggerService)
    {
        _shiftLoggerService = shiftLoggerService;
    }

    [HttpPost("shift-in")]
    public async Task<ActionResult> ShiftIn([FromBody] ShiftInDto shiftInDto)
    {
        try
        {
            await _shiftLoggerService.ShiftIn(shiftInDto);
            return Ok("Shift started successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error during shift-in operation.", details = ex.Message });
        }
    }

    [HttpPost("shift-out")]
    public async Task<ActionResult> ShiftOut([FromBody] ShiftOutDto shiftOutDto)
    {
        try
        {
            await _shiftLoggerService.ShiftOut(shiftOutDto);
            return Ok("Shift ended successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error during shift out operation.", details = ex.Message });
        }
    }
    [HttpGet("employeelist")]
    public async Task<ActionResult> EmployeeList()
    {
        try
        {
            var employeeList = await _shiftLoggerService.EmployeeList();
            return Ok(employeeList);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error during obtaining employee list.", details = ex.Message });
        }
    }
    [HttpGet("shiftlog")]
    public async Task<ActionResult> ShiftLog()
    {
        try
        {
            var shiftLog = await _shiftLoggerService.ShiftLog();
            return Ok(shiftLog);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error during obtaining employee list.", details = ex.Message });
        }
    }
    [HttpGet("checkshiftstatus")]
    public async Task<IActionResult> CheckShiftStatus(int employeeId)
    {
        try
        {
            bool shiftLog = await _shiftLoggerService.CheckShiftStatus(employeeId);

            return Ok(shiftLog);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error during obtaining employee list.", details = ex.Message });
        }
    }
}

