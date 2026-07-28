using ShiftsLogger.UI;
using ShiftsLogger.UI.Services;

using var client = new HttpClient();
string urlBase = "http://localhost:5062";
var shiftApiService = new ShiftApiService(client, urlBase);
var workerApiService = new WorkerApiService(client, urlBase);

await UserInput.RunAsync(workerApiService, shiftApiService);
