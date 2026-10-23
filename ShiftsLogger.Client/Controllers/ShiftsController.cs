using System.Net;

public sealed class ShiftsController
{
    private readonly IShiftsService _shiftsService;
    private readonly IEmployeesService _employeesService;
    private readonly ShiftsView _view;
    private string? _pendingMessage;

    public ShiftsController(
        IShiftsService shiftsService,
        IEmployeesService employeesService,
        ShiftsView view)
    {
        _shiftsService = shiftsService;
        _employeesService = employeesService;
        _view = view;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            IReadOnlyCollection<ClientShift> shifts;
            try
            {
                shifts = await _shiftsService.GetAllAsync();
            }
            catch (ApiClientException exception)
            {
                _view.DisplayMessage(exception.Message);
                _view.WaitForInput();
                return;
            }

            _view.DisplayShifts(shifts);
            if (_pendingMessage is not null)
            {
                _view.DisplayMessage(_pendingMessage);
                _pendingMessage = null;
            }

            switch (_view.DisplayManagementMenu())
            {
                case ManagementMenuOption.Add:
                    await AddAsync();
                    break;

                case ManagementMenuOption.Update:
                    await UpdateAsync();
                    break;

                case ManagementMenuOption.Delete:
                    await DeleteAsync();
                    break;

                case ManagementMenuOption.Back:
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(ManagementMenuOption),
                        "Unknown shift menu option.");
            }
        }
    }

    private async Task AddAsync()
    {
        var employeeId = _view.PromptEmployeeId();
        if (!await IsEmployeeAssignableAsync(employeeId))
            return;

        var request = _view.PromptShiftTimes(employeeId);
        await RunOperationAsync(
            () => _shiftsService.CreateAsync(request),
            "Shift created.");
    }

    private async Task UpdateAsync()
    {
        var id = _view.PromptShiftId("update");
        ClientShift shift;
        try
        {
            shift = await _shiftsService.GetByIdAsync(id);
        }
        catch (ApiClientException exception)
        {
            _view.DisplayMessage(exception.Message);
            _view.WaitForInput();
            return;
        }

        var employeeId = _view.PromptEmployeeId(shift.EmployeeId);
        if (employeeId != shift.EmployeeId && !await IsEmployeeAssignableAsync(employeeId))
            return;

        var request = _view.PromptShiftTimes(employeeId, shift);
        await RunOperationAsync(
            () => _shiftsService.UpdateAsync(id, request),
            "Shift updated.");
    }

    private async Task DeleteAsync()
    {
        var id = _view.PromptShiftId("delete");
        await RunOperationAsync(
            () => _shiftsService.DeleteAsync(id),
            "Shift deleted.");
    }

    private async Task RunOperationAsync(Func<Task> operation, string successMessage)
    {
        try
        {
            await operation();
            _view.DisplayMessage(successMessage);
        }
        catch (ApiClientException exception)
        {
            _pendingMessage = exception.StatusCode == HttpStatusCode.BadRequest
                && exception.ResponseDetail?.Contains("Cannot assign shift", StringComparison.OrdinalIgnoreCase) == true
                    ? exception.ResponseDetail
                    : exception.Message;
        }

        if (_pendingMessage is null)
            _view.WaitForInput();
    }

    private async Task<bool> IsEmployeeAssignableAsync(int employeeId)
    {
        try
        {
            var employee = await _employeesService.GetByIdAsync(employeeId);
            if (employee.IsActive)
                return true;

            _pendingMessage = "The employee is not active. Cannot assign shift.";
            return false;
        }
        catch (ApiClientException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            _pendingMessage = $"Employee {employeeId} was not found. Enter an existing active employee.";
            return false;
        }
        catch (ApiClientException exception)
        {
            _pendingMessage = exception.Message;
            return false;
        }
    }
}
