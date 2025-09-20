using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Interfaces;
using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Controllers;
[ApiController]
[Route("api/shifts")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftsController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpGet]
    public ActionResult<List<Shift>> GetAllShifts()
    {
        return Ok(_shiftService.GetAllShifts());
    }

    [HttpGet("{id}")]
    public ActionResult<Shift> GetShiftById(int id)
    {
        return Ok(_shiftService.GetShiftById(id));
    }

    [HttpPost]
    public ActionResult<Shift> CreateShift(Shift shift)
    {
        return Ok(_shiftService.CreateShift(shift));
    }

    [HttpPut("{id}")]
    public ActionResult<Shift> UpdateShift(int id, Shift updatedShift)
    {
        return Ok(_shiftService.UpdateShift(id, updatedShift));
    }

    [HttpDelete("{id}")]
    public ActionResult<string> DeleteShift(int id)
    {
        return Ok(_shiftService?.DeleteShift(id));
    }
}
