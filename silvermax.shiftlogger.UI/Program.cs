

using Microsoft.Extensions.DependencyInjection;
using silvermax.shiftlogger.UI;

internal class Program
{
    public async static Task Main(string[] args)
    {
        var services = new ServiceCollection();
        var provider = services.ServiceProvider();

        UserInterface ui = provider.GetRequiredService<UserInterface>();

        await ui.Start();
    }
}
