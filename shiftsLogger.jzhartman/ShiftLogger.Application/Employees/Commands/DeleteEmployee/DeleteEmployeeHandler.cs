using ShiftLogger.Domain.Models;
using ShiftLogger.Domain.Validation;
using ShiftLogger.Domain.Validation.Errors;
using ShiftLogger.Infrastructure.Repositories;

namespace ShiftLogger.Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeHandler
{
    private readonly IEmployeeRepository _empoyeeRepository;
    private readonly IShiftsRepository _shiftRepository;

    public DeleteEmployeeHandler(IEmployeeRepository empoyeeRepository, IShiftsRepository shiftRepository)
    {
        _empoyeeRepository = empoyeeRepository;
        _shiftRepository = shiftRepository;
    }

    public async Task<Result> HandleAsync(DeleteEmployeeCommand command)
    {
        var employeeToDelete = new Employee
        {
            Id = command.Id,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        var employeeExistsResult = await _empoyeeRepository.EmployeeExistsByIdAsync(employeeToDelete.Id);
        if (!employeeExistsResult.Value)
            return Result.Failure(Errors.EmployeeNotFound);
        if (employeeExistsResult.IsFailure)
            return Result.Failure(employeeExistsResult.Errors);

        var employeeResult = await _empoyeeRepository.GetEmployeeByIdAsync(employeeToDelete.Id);
        if (employeeResult is null || employeeResult.Value is null)
            return Result.Failure(Errors.EmployeeNotFound);
        if (employeeToDelete.FirstName != employeeResult.Value.FirstName || employeeToDelete.LastName != employeeResult.Value.LastName)
            return Result.Failure(Errors.RecordMismatch);

        var shiftCountResult = await _shiftRepository.ShiftCountByEmployeeIdAsync(employeeToDelete.Id);
        if (shiftCountResult.IsFailure)
            return Result.Failure(shiftCountResult.Errors);

        if (shiftCountResult.Value > 0)
        {
            var deleteShiftsResult = await _shiftRepository.DeleteAllShiftsByEmployeeIdAsync(command.Id);
            if (deleteShiftsResult.IsFailure)
                return Result.Failure(deleteShiftsResult.Errors);
        }

        return await _empoyeeRepository.DeleteEmployeeAsync(employeeToDelete);
    }
}