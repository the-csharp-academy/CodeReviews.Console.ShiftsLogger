using ShiftsLogger.DTOs;

namespace ShiftsLogger.Services;

public interface IWorkerService
{
    public Task<List<WorkerReadDTO>> GetAllWorkersAsync();
    public Task<WorkerReadDTO?> GetWorkerByIdAsync(int id);
    public Task<WorkerReadDTO> CreateWorkerAsync(WorkerDTO worker);
    public Task<WorkerReadDTO?> UpdateWorkerAsync(int id, WorkerDTO newWorker);
    public Task<bool> DeleteWorkerAsync(int id);
}
