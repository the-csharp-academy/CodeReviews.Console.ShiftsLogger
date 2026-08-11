using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Data;
using ShiftsLogger.Models;
using ShiftsLogger.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ShiftsLogger.Services;

public class WorkerService : IWorkerService
{
    private readonly ShiftsLoggerContext _context;
    private readonly IMapper _mapper;

    public WorkerService(ShiftsLoggerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkerReadDTO>> GetAllWorkersAsync()
    {
        return await _context.Workers
            .AsNoTracking()
            .ProjectTo<WorkerReadDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<WorkerReadDTO?> GetWorkerByIdAsync(int id)
    {
        return await _context.Workers
            .AsNoTracking()
            .Where(w => w.Id == id)
            .ProjectTo<WorkerReadDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<WorkerReadDTO> CreateWorkerAsync(WorkerDTO dto)
    {
        var worker = _mapper.Map<Worker>(dto);

        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();

        return _mapper.Map<WorkerReadDTO>(worker);
    }

    public async Task<WorkerReadDTO?> UpdateWorkerAsync(int id, WorkerDTO dto)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
        {
            return null;
        }

        _mapper.Map(dto, worker);
        await _context.SaveChangesAsync();

        return _mapper.Map<WorkerReadDTO>(worker);

    }

    public async Task<bool> DeleteWorkerAsync(int id)
    {
        var worker = await _context.Workers.FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
        {
            return false;
        }

        _context.Remove(worker);
        await _context.SaveChangesAsync();
        return true;
    }
}
