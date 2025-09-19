using Microsoft.AspNetCore.Mvc;
using ShiftLoggerAPI.Contracts;

namespace ShiftLoggerAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftsController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }
}
