public sealed class EmployeesController
{
    private readonly IEmployeesService _employeesService;
    private readonly EmployeesView _view;

    public EmployeesController(IEmployeesService employeesService, EmployeesView view)
    {
        _employeesService = employeesService;
        _view = view;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            IReadOnlyCollection<ClientEmployee> employees;
            try
            {
                employees = await _employeesService.GetAllAsync();
            }
            catch (ApiClientException exception)
            {
                _view.DisplayMessage(exception.Message);
                _view.WaitForInput();
                return;
            }

            _view.DisplayEmployees(employees);
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
                        "Unknown employee menu option.");
            }
        }
    }

    private async Task AddAsync()
    {
        var request = _view.PromptEmployeeDetails();
        await RunOperationAsync(
            () => _employeesService.CreateAsync(request),
            "Employee created.");
    }

    private async Task UpdateAsync()
    {
        var id = _view.PromptEmployeeId("update");
        ClientEmployee employee;
        try
        {
            employee = await _employeesService.GetByIdAsync(id);
        }
        catch (ApiClientException exception)
        {
            _view.DisplayMessage(exception.Message);
            _view.WaitForInput();
            return;
        }

        var request = _view.PromptEmployeeUpdate(employee);
        await RunOperationAsync(
            () => _employeesService.UpdateAsync(id, request),
            "Employee updated.");
    }

    private async Task DeleteAsync()
    {
        var id = _view.PromptEmployeeId("delete");
        await RunOperationAsync(
            () => _employeesService.DeleteAsync(id),
            "Employee deleted.");
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
            _view.DisplayMessage(exception.Message);
        }

        _view.WaitForInput();
    }
}
