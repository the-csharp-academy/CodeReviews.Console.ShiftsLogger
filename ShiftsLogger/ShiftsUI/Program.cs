using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ShiftsUI.Infrastructure;
using ShiftsUI.Controllers;
using ShiftsUI.Views;
using ShiftsUI.Services;

namespace ShiftsUI;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var collection = new ServiceCollection();

        collection.AddScoped<UIController>();
        collection.AddScoped<Router>();
        collection.AddScoped<IShiftView, ShiftView>();
        collection.AddScoped<IShiftUIService, ShiftUIService>();

        var provider = collection.BuildServiceProvider();

        var uiController = provider.GetRequiredService<UIController>();
        var router = provider.GetRequiredService<Router>();
        await router.Route(uiController);
    }
}