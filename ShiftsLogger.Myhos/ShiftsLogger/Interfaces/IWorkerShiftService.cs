using ShiftsLogger.DTOs;
using ShiftsLogger.Model;

namespace ShiftsLogger.Interfaces;

public interface IWorkerShiftService
{
    public List<Shift> GetWorkerShifts();
    public Shift? GetWorkerShitftById(int id);
    public Shift? UpdateWorkerShift(int id,UpdateWorkerShiftDto dto);
    public Shift CreateWorkerShift(CreatedWorkerShiftDTO dto);
    public bool DeleteWorkerShift(int id); 
}