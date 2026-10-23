namespace ShiftsLogger.Client;

public sealed class AppController
{
    private readonly IAppView _appView;
    private readonly ShiftsController _shiftsController;
    private readonly EmployeesController _employeesController;

    public AppController(
        IAppView view,
        ShiftsController shiftsController,
        EmployeesController employeesController)
    {
        _appView = view;
        _shiftsController = shiftsController;
        _employeesController = employeesController;
    }

    public async Task RunAsync()
    {
        bool isRunning = true;

        while (isRunning)
        {
            MainMenuOption selectedOption = _appView.DisplayMainMenu();

            switch (selectedOption)
            {
                case MainMenuOption.EnterPortal:
                    await EnterPortalAsync();
                    break;

                case MainMenuOption.Exit:
                    isRunning = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(selectedOption),
                        selectedOption,
                        "Unknown menu option.");
            }
        }
        _appView.DisplayGoodbye();
    }

    private async Task EnterPortalAsync()
    {
        bool isInPortal = true;

        while (isInPortal)
        {
            switch (_appView.DisplayPortalMenu())
            {
                case PortalMenuOption.Shifts:
                    await _shiftsController.RunAsync();
                    break;

                case PortalMenuOption.Employees:
                    await _employeesController.RunAsync();
                    break;

                case PortalMenuOption.Back:
                    isInPortal = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(PortalMenuOption),
                        "Unknown portal menu option.");
            }
        }
    }
}