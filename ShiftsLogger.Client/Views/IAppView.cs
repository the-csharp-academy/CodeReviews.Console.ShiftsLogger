public interface IAppView
{
    MainMenuOption DisplayMainMenu();
    PortalMenuOption DisplayPortalMenu();
    void DisplayMessage(string message);
    void DisplayGoodbye();
    void WaitForInput();
}