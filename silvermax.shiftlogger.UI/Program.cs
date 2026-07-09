

using silvermax.shiftlogger.UI;

internal class Program
{
    public async static Task Main(string[] args)
    {
        UserInterface ui = new();

        await ui.Start();
    }
}
