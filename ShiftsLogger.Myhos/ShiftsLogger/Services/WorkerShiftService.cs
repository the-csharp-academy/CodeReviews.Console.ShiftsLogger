using ShiftsLogger.Data;
using ShiftsLogger.DTOs;
using ShiftsLogger.Exceptions;
using ShiftsLogger.Interfaces;
using ShiftsLogger.Model;

namespace ShiftsLogger.Services;

public class WorkerShiftService : IWorkerShiftService
{
    private readonly ShiftsLoggerContextDB _contextDB;
    private readonly ILogger<WorkerShiftService> _logger;

    public WorkerShiftService(ShiftsLoggerContextDB contextDB, ILogger<WorkerShiftService> logger) 
    {
       _contextDB = contextDB;
       _logger = logger;
    }

    public Shift CreateWorkerShift(CreatedWorkerShiftDTO dto)
    {
        if (dto.StartTime == dto.EndTime)
            throw new ValidationException("Star time and End time cannot be the same");

        Shift workerShift = new()
        {
            WorkerName = dto.WorkerName,
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            ShiftTime = dto.ShiftTime,
            HoursWorked = CalculateHoursWorked(dto.StartTime, dto.EndTime),
            CreatedAt = DateTime.Now
        };

        workerShift.UpdateAt = GetUpdateAt(workerShift.CreatedAt);

        _contextDB.WorkerShifts.Add(workerShift);
        _contextDB.SaveChanges();

        _logger.LogInformation("Worker Shift {Id} created", workerShift.ShiftId);

        return workerShift;
    }

    public void DeleteWorkerShift(int id)
    {
        Shift? workerShift = _contextDB.WorkerShifts.Find(id);

        if(workerShift == null) 
        {
            _logger.LogWarning("Worker shift {Id} not found.", id);
            throw new NotFoundException($"Worker shift with id {id} was not found.");
        }

        _contextDB.WorkerShifts.Remove(workerShift);
        _contextDB.SaveChanges();

        _logger.LogInformation("Worker shift {Id} deleted.",id);
    }

    public List<Shift> GetWorkerShifts()
    {
        List<Shift> workerShifts = _contextDB.WorkerShifts.ToList();

        _logger.LogInformation("Retrieved {Count} worker shifts.",workerShifts.Count);

        return workerShifts;
    }

    public Shift? GetWorkerShitftById(int id)
    {
        Shift? workerShift = _contextDB.WorkerShifts.Find(id);

        if (workerShift == null) 
        {
            _logger.LogWarning("Worker shift with Id {Id} was not found.", id);
            throw new NotFoundException($"Worker shift with id {id} was not found.");
        }

        _logger.LogInformation("Worker shift with Id {Id} retrieved.",id);

        return workerShift;
    }

    public Shift? UpdateWorkerShift(int id, UpdateWorkerShiftDto dto)
    {
        Shift? workerShift = _contextDB.WorkerShifts.Find(id);

        if (workerShift == null) 
        {
            _logger.LogWarning("Worker shift {Id} not found", id);
            throw new NotFoundException($"Worker shift  with id {id} was not found.");
        }

        if (dto.WorkerName != null)
            workerShift.WorkerName = dto.WorkerName;

        if (dto.Date.HasValue)
            workerShift.Date = dto.Date.Value;

        if (dto.StartTime.HasValue)
            workerShift.StartTime = dto.StartTime.Value;

        if (dto.EndTime.HasValue)
            workerShift.EndTime = dto.EndTime.Value;

        if (dto.ShiftTime != null)
            workerShift.ShiftTime = dto.ShiftTime;

        workerShift.HoursWorked = CalculateHoursWorked(workerShift.StartTime,workerShift.EndTime);
        workerShift.UpdateAt = GetUpdateAt(workerShift.CreatedAt);

        _contextDB.SaveChanges();

        _logger.LogInformation("Worker shift {Id} updated.",id);

        return workerShift;
    }

    private static decimal CalculateHoursWorked(TimeOnly start, TimeOnly end)
    {
        TimeSpan duration;

        if (end >= start)
            duration = end - start;
        else
            duration = (TimeOnly.MaxValue - start) + end.ToTimeSpan() + TimeSpan.FromMinutes(1);

        return (decimal)duration.TotalHours;
    }

    private DateTime GetUpdateAt(DateTime createdAt)
    {
        DateTime now = DateTime.Now;
        DateTime minimumUpdateTime = createdAt.AddMinutes(20);

        return now < minimumUpdateTime
            ? minimumUpdateTime
            : now;
    }
}