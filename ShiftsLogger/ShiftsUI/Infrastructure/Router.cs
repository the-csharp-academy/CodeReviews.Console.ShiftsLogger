using ShiftsUI.Controllers;

namespace ShiftsUI.Infrastructure;

internal class Router
{
    internal async Task Route(UIController uiController)
    {
        while (true)
        {
            await uiController.HandleMainMenu();
        }
    }
}