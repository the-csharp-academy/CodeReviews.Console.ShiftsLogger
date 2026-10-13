using Microsoft.AspNetCore.Mvc;
using ShiftsLoggerApi.DTOs;
using ShiftsLoggerApi.Models;

namespace ShiftsLoggerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftService shiftService;

        public ShiftsController(ShiftService shiftServiceAssign)
        {
            shiftService = shiftServiceAssign;
        }

        [HttpGet(Name = "ShiftsLoggerAdress")]
        public IEnumerable<ShiftsLog> Get()
        {
            return shiftService.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var newShift = shiftService.GetById(id);
            if (newShift.ShiftError == ShiftError.InvalidId)
            {
                return BadRequest($" Id {id} must be more than 0");
            }
            if (newShift.ShiftError == ShiftError.NotFound)
            {
                return NotFound($"Id {id} doesn't exist in database");
            }
            return Ok(newShift.Shift);
        }

        [HttpPost]
        public IActionResult Create(ShiftDto shiftDto)
        {
            var newShift = shiftService.Create(shiftDto);
            if (newShift.ShiftError == ShiftError.InvalidDate)
            {
                return BadRequest("End Date must be later than Start Date");
            }
            return Ok(newShift.Shift);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ShiftDto shiftDto)
        {
            var newShift = shiftService.Put(id, shiftDto);
            if (newShift.ShiftError == ShiftError.InvalidId)
            {
                return BadRequest($"Id {id} must be more than 0");
            }
            if (newShift.ShiftError == ShiftError.InvalidDate)
            {
                return BadRequest($"End Date must be later than Start Date");
            }
            if (newShift.ShiftError == ShiftError.NotFound)
            {
                return NotFound($"Id {id} doesn't exist in database");
            }
            return Ok(newShift.Shift);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var newShift = shiftService.Delete(id);
            if (newShift.ShiftError == ShiftError.InvalidId)
            {
                return BadRequest($"Id {id} must be more than 0 ");
            }
            if (newShift.ShiftError == ShiftError.NotFound)
            {
                return NotFound($"Id {id} doesn't exist in database");
            }
            return Ok(newShift.Shift);
        }
    }
}