using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.Services;
using ShiftsLogger.DTOs;

namespace ShiftsLogger.Controllers
{
    [ApiController]
    [Route("api/workers")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        private readonly IShiftService _shiftService;

        public WorkerController(IWorkerService workerService, IShiftService shiftService)
        {
            _workerService = workerService;
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkersAsync()
        {
            var result = await _workerService.GetAllWorkersAsync();
            
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerAsync(int id)
        {
            var result = await _workerService.GetWorkerByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{workerId}/shifts")]
        public async Task<IActionResult> GetAllShiftsByWorkerAsync(int workerId)
        {
            var result = await _shiftService.GetAllShiftsByWorkerAsync(workerId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkerAsync(WorkerDTO dto)
        {
            var result = await _workerService.CreateWorkerAsync(dto);

            if (result == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetWorker", new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkerAsync(int id, WorkerDTO dto)
        {
            var result = await _workerService.UpdateWorkerAsync(id, dto);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkerAsync(int id)
        {
            var result = await _workerService.DeleteWorkerAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
