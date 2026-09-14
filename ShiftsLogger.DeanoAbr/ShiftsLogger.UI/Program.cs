using ShiftsLogger.UI.Menu;
using ShiftsLogger.UI.Services;

var baseUrl = Environment.GetEnvironmentVariable("SHIFTSLOGGER_API_URL") ?? "http://localhost:5273/";

// A BaseAddress without a trailing slash makes relative paths resolve incorrectly.
if (!baseUrl.EndsWith('/'))
{
    baseUrl += "/";
}

using var http = new HttpClient
{
    BaseAddress = new Uri(baseUrl),
    Timeout = TimeSpan.FromSeconds(10)
};

var client = new ShiftApiClient(http);
var app = new ShiftConsoleApp(client, Console.In, Console.Out);

await app.RunAsync();
return 0;
