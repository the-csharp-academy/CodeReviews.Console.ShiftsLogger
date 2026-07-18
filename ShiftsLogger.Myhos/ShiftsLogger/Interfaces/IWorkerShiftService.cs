using ShiftsLogger.DTOs;
using ShiftsLogger.Model;

namespace ShiftsLogger.Interfaces;

public interface IWorkerShiftService
{
    public List<WorkerShift> GetWorkerShifts();
    public WorkerShift? GetWorkerShitftById(int id);
    public WorkerShift UpdateWorkerShift(int id,UpdateWorkerShiftDto dto);
    public WorkerShift CreateWorkerShift(CreatedWorkerShiftDTO dto);
    public bool DeleteWorkerShift(int id); 
}