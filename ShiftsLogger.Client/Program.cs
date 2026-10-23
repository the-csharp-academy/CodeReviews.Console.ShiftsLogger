using Microsoft.Extensions.Configuration;

namespace ShiftsLogger.Client;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var config = Setup();

            string baseUrl = config["ShiftsDB:BaseUrl"]
                ?? throw new InvalidOperationException("ShiftsDB:BaseUrl is missing.");

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/"),
                Timeout = TimeSpan.FromSeconds(15)
            };

            IAppView appView = new AppView();
            IShiftsService shiftsService = new ShiftsService(httpClient);
            IEmployeesService employeesService = new EmployeesService(httpClient);
            var shiftsController = new ShiftsController(
                shiftsService,
                employeesService,
                new ShiftsView());
            var employeesController = new EmployeesController(employeesService, new EmployeesView());
            var mainApp = new AppController(appView, shiftsController, employeesController);

            await mainApp.RunAsync();
        }
        catch (Exception exception)
        {
            var message = string.Join(
                " ",
                exception.Message.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
            if (message.Length > 400)
                message = $"{message[..400]}...";

            Console.Error.WriteLine(
                $"The client stopped unexpectedly ({exception.GetType().Name}): {message}");
            Environment.ExitCode = 1;
        }
    }

    private static IConfigurationRoot Setup()
    {
        IConfigurationRoot config =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "Configuration/appsettings.json",
                    optional: false)
                .Build();

        return config;
    }
}