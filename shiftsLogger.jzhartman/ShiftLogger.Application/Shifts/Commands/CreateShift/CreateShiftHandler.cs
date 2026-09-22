using ShiftLogger.Domain.Models;
using ShiftLogger.Domain.Validation;
using ShiftLogger.Domain.Validation.Errors;
using ShiftLogger.Infrastructure.Repositories;

namespace ShiftLogger.Application.Shifts.Commands.CreateShift;

public class CreateShiftHandler
{
    private readonly IShiftsRepository _shiftsRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public CreateShiftHandler(IShiftsRepository shiftsRepository, IEmployeeRepository employeeRepository)
    {
        _shiftsRepository = shiftsRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> HandleAsync(CreateShiftCommand command)
    {
        var newShift = new Shift()
        {
            EmployeeId = command.EmployeeId,
            ClockInTime = command.ClockInTime,
            ClockOutTime = command.ClockOutTime
        };

        var employeeExistsResult = await _employeeRepository.EmployeeExistsByIdAsync(newShift.EmployeeId);
        if (!employeeExistsResult.Value)
            return Result.Failure(Errors.EmployeeNotFound);
        if (employeeExistsResult.IsFailure)
            return Result.Failure(employeeExistsResult.Errors);

        if (command.ClockInTime >= command.ClockOutTime)
            return Result.Failure(Errors.ClockInTimePrecedesClockOutTime);

        if (command.ClockInTime >= DateTime.Now || command.ClockOutTime > DateTime.Now)
            return Result.Failure(Errors.CannotLogFutureShifts);

        var overlapResult = await _shiftsRepository.OverlapsExistingShiftAsync(newShift);
        if (overlapResult.Value)
            return Result.Failure(Errors.NewShiftOverlapsExistingShift);
        if (overlapResult.IsFailure)
            return Result.Failure(overlapResult.Errors);

        var createResult = await _shiftsRepository.CreateShiftAsync(newShift);
        if (createResult.IsFailure)
            return Result.Failure(createResult.Errors);

        return await _shiftsRepository.SaveChangesAsync();
    }
}