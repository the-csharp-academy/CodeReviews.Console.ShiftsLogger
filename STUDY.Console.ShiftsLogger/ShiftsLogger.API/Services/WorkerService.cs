using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Models.DTOs.shift;
using ShiftsLogger.API.Models.DTOs.Worker;

namespace ShiftsLogger.API.Services;

public interface IWorkerService
{
    Task<WorkerDto?> AddWorkerAsync(WorkerCreationDto workerCreationDtoDto);
    Task<WorkerDto?> GetWorkerByIdAsync(int id);
    Task<WorkerDto?> UpdateWorkerAsync(int id, WorkerCreationDto updatedWorkerDto);
    Task<bool> DeleteWorkerAsync(int id);
    Task<List<WorkerDto>?> GetAllWorkersAsync();
    Task<WorkerWithShiftsDto?> GetWorkerWithShiftsAsync(int id);
}
public class WorkerService : IWorkerService
{
    private readonly AppDbContext _context;
    private readonly IShiftService _shiftService;
    public WorkerService(AppDbContext context, IShiftService shiftService)
    {
        _context = context;
        _shiftService = shiftService;
    }
    public async Task<WorkerDto?> AddWorkerAsync(WorkerCreationDto workerCreationDtoDto)
    {
        if (string.IsNullOrWhiteSpace(workerCreationDtoDto.Name))
        {
            return null;
        }

        Worker worker = new()
        {
            Name = workerCreationDtoDto.Name
        };
        await _context.Workers.AddAsync(worker);
        await _context.SaveChangesAsync();
        WorkerDto workerDto = new()
        {
            Id = worker.Id,
            Name = worker.Name
        };
        return workerDto;
    }

    public async Task<bool> DeleteWorkerAsync(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker is null)
        {
            return false;
        }
        //TODO: Set OnDelete to SetNull in DbContext
        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<WorkerDto>?> GetAllWorkersAsync()
    {
        var workers = _context.Workers;
        var result = await workers.Select(w => new WorkerDto
        {
            Id = w.Id,
            Name = w.Name
        }).ToListAsync();
        return result;
    } //DONE

    public async Task<WorkerWithShiftsDto?> GetWorkerWithShiftsAsync(int id)
    {
        var worker = _context.Workers.Include(w => w.Shifts).Where(w => w.Id == id);
        var shifts = await _shiftService.GetShiftsByWorkerIdAsync(id);
        

        var result = worker.Select(w => new WorkerWithShiftsDto
        {
            WorkerId = w.Id,
            Name = w.Name,
            Shifts = shifts ?? new List<ShiftDto>()
        }).SingleOrDefault();

        return result;
    }

    public async Task<WorkerDto?> GetWorkerByIdAsync(int id)
    {
        var worker =  await _context.Workers.FindAsync(id);
        if (worker is null)
        {
            return null;
        }

        WorkerDto workerDto = new()
        {
            Id = worker.Id,
            Name = worker.Name
        };
        return workerDto;
    }

    public async Task<WorkerDto?> UpdateWorkerAsync(int id, WorkerCreationDto updatedWorkerDto)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker is null)
        {
            return null;
        }
        worker.Name = updatedWorkerDto.Name;
        await _context.SaveChangesAsync();
        WorkerDto updatedWorker = new()
        {
            Id = worker.Id,
            Name = worker.Name
        };
        return updatedWorker;
    }
}
