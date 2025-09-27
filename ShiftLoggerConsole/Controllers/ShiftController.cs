using ShiftLoggerConsole.Models;
using ShiftLoggerConsole.Services;

namespace ShiftLoggerConsole.Controllers;
internal class ShiftController
{
    private ManagementService _client;

    public ShiftController()
    {
        _client = new ManagementService();
    }

    public async Task<List<Shift>> GetAllShifts()
    {
        string endpoint = "shifts";
        var res = await _client.GetAsync<List<Shift>>(endpoint);

        return res ?? new List<Shift>();
    }
}
