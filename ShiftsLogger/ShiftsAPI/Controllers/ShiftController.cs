using Microsoft.AspNetCore.Mvc;
using ClassLibray.Models;
using ShiftsAPI.Services;

namespace ShiftsAPI.Controllers;

[ApiController]
[Route("api/shifts")]
public class ShiftController : ControllerBase
{
    private readonly IShiftAPIService _shiftService;
    public ShiftController(IShiftAPIService ShiftService)
    {
        _shiftService = ShiftService;
    }

    [HttpPost]
    public Shift CreateShift(Shift shift)
    {
        return _shiftService.CreateShift(shift);
    }

    [HttpGet]
    public List<Shift> GetAllShifts()
    {
        return _shiftService.GetAllShifts();
    }

    [HttpGet("{id}")]
    public Shift? GetShiftById(int id)
    {
        return _shiftService.GetShiftById(id);
    }

    [HttpPut("{id}")]
    public Shift? UpdateShift(int id, Shift updatedShift)
    {
        return _shiftService.UpdateShift(id, updatedShift);
    }

    [HttpDelete("{id}")]
    public string DeleteShift(int id)
    {
        return _shiftService.DeleteShift(id);
    }
}