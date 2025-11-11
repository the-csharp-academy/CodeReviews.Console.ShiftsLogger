using ShiftLogger.DataAccess;
using ShiftLogger.Utils;
using System.Diagnostics.Eventing.Reader;
using static ShiftLogger.Models.ShiftLoggerDto.ShiftLoggerDto;
using static ShiftLogger.Models.ShiftLoggerEntity.ShifLoggerEntity;

namespace ShiftLogger.Service;

public interface IShiftLoggerService
{
    Task<List<EmployeeListDto>> EmployeeList();
    Task ShiftIn(ShiftInDto shiftInDto);
    Task ShiftOut(ShiftOutDto shiftOutDto);
    Task<List<ShiftLogDto>> ShiftLog();
    Task <bool> CheckShiftStatus(int employeeId);
}

public class ShiftLoggerService : IShiftLoggerService
{
    public readonly IShiftLoggerDataAccess _shiftLoggerDataAccess;
    private readonly ServiceUtils _serviceUtils;

    public ShiftLoggerService(IShiftLoggerDataAccess shiftLoggerDataAccess, ServiceUtils serviceUtils)
    {
        _shiftLoggerDataAccess = shiftLoggerDataAccess;
        _serviceUtils = serviceUtils;
    }

    public async Task ShiftIn(ShiftInDto shiftInDto)
    {
        try
        {
            var shiftIn = new ShiftDetails { EmployeeId = shiftInDto.EmployeeId, ShiftStart = DateTime.Now, ShiftStatus = 1 };
            await _shiftLoggerDataAccess.ShiftIn(shiftIn);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error during shift in operation.", ex);
        }
    }

    public async Task ShiftOut(ShiftOutDto shiftOutDto)
    {
        try
        {
            var shiftOut = new ShiftDetails { EmployeeId = shiftOutDto.EmployeeId, ShiftEnd = DateTime.Now, TotalWorkingHours = _serviceUtils.GetTotalWorkingHours(shiftOutDto.EmployeeId, DateTime.Now), ShiftStatus = 0 };
            await _shiftLoggerDataAccess.ShiftOut(shiftOut);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error during shift out operation.", ex);
        }
    }
    public async Task<List<EmployeeListDto>> EmployeeList()
    {
        try
        {
            var employeelist = await _shiftLoggerDataAccess.EmployeeList();

            return employeelist.Select(e => new EmployeeListDto
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error during obtaining employee list.", ex);
        }
    }

    public async Task<List<ShiftLogDto>> ShiftLog()
    {
        try
        {
            var shiftLog = await _shiftLoggerDataAccess.ShiftLog();

            return shiftLog.OrderByDescending(e => e.CreatedDate)
                            .Select(e => new ShiftLogDto
                            {
                                Name = e.Employee.Name,
                                TotalWorkingHours = e.TotalWorkingHours,
                                ShiftStatus = e.ShiftStatus
                            }).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error during obtaining employee list.", ex);
        }
    }

    public async Task<bool> CheckShiftStatus(int employeeId)
    {
        bool isShiftActive = false;

        var shiftStatus = await _shiftLoggerDataAccess.CheckShiftStatus(employeeId);

        if (shiftStatus == 1)
        {
            isShiftActive = true;
        }

        return isShiftActive;
    }

}
