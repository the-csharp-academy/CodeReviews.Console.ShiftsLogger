using Microsoft.EntityFrameworkCore;

namespace ShiftLogger;

public class ShiftService
{

    private readonly ShiftLoggerContext _context;

    public ShiftService (ShiftLoggerContext context)
    {
        _context = context;
    }
    public bool ValidateShift(DateTime startTime, DateTime endtime)
    {
        if (endtime <= startTime)
            return false;

        return true;
    }

    public TimeSpan CalculateDuration(DateTime startTime, DateTime endtime)
    {
        TimeSpan duration = endtime - startTime;

        return duration;
    }

    public async Task<bool> WorkerExists(int workerId)
    {
        bool worker = await _context.Workers.AnyAsync(w => w.Id == workerId);

        return worker;
    }
}