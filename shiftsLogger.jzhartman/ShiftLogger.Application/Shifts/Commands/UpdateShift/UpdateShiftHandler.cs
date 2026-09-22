using ShiftLogger.Domain.Models;
using ShiftLogger.Domain.Validation;
using ShiftLogger.Domain.Validation.Errors;
using ShiftLogger.Infrastructure.Repositories;

namespace ShiftLogger.Application.Shifts.Commands.UpdateShift;

public class UpdateShiftHandler
{
    private readonly IShiftsRepository _shiftsRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateShiftHandler(IShiftsRepository shiftsRepository, IEmployeeRepository employeeRepository)
    {
        _shiftsRepository = shiftsRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> HandleAsync(UpdateShiftCommand shift)
    {
        var updatedShift = new Shift
        {
            Id = shift.Id,
            EmployeeId = shift.EmployeeId,
            ClockInTime = shift.ClockInTime,
            ClockOutTime = shift.ClockOutTime
        };

        var employeeExistsResult = await _employeeRepository.EmployeeExistsByIdAsync(updatedShift.EmployeeId);
        if (!employeeExistsResult.Value)
            return Result.Failure(Errors.EmployeeNotFound);
        if (employeeExistsResult.IsFailure)
            return Result.Failure(employeeExistsResult.Errors);

        var shiftExistsResult = await _shiftsRepository.ShiftExistsByIdAsync(updatedShift.Id);
        if (!shiftExistsResult.Value)
            return Result.Failure(Errors.ShiftIdNotFound);
        if (shiftExistsResult.IsFailure)
            return Result.Failure(shiftExistsResult.Errors);

        if (updatedShift.ClockInTime >= updatedShift.ClockOutTime)
            return Result.Failure(Errors.ClockInTimePrecedesClockOutTime);

        if (updatedShift.ClockInTime >= DateTime.Now || updatedShift.ClockOutTime > DateTime.Now)
            return Result.Failure(Errors.CannotLogFutureShifts);

        var overlapsResult = await _shiftsRepository.OverlapsExistingShiftsExcludingCurrentAsync(updatedShift);
        if (overlapsResult.Value)
            return Result.Failure(Errors.NewShiftOverlapsExistingShift);
        if (overlapsResult.IsFailure)
            return Result.Failure(overlapsResult.Errors);

        var updateResult = await _shiftsRepository.UpdateShiftByIdAsync(updatedShift);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Errors);

        return await _shiftsRepository.SaveChangesAsync();
    }
}
