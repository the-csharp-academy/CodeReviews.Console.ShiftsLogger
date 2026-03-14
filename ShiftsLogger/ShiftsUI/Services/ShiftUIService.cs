using System.Globalization;
using System.Net.Http.Json;
using ClassLibray.Models;

namespace ShiftsUI.Services;

internal interface IShiftUIService
{
    Task Create(Shift shift);
    Task<List<Shift>> ReadAll();
    Task Update(int id, Shift updatedShift);
    Task Delete(int id);
    bool ValidDateTime(string input);
    string FormatDateTime(string time);
    string FormatDuration(TimeSpan duration);
}
internal class ShiftUIService : IShiftUIService
{
    private HttpClient _client = new()
    {
        BaseAddress = new Uri("http://localhost:5098")
    }; 

    public async Task Create(Shift shift)
    {
        var response = await _client.PostAsJsonAsync("api/shifts", shift);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Shift>> ReadAll()
    {
        return await _client.GetFromJsonAsync<List<Shift>>("api/shifts");
    }

    public async Task Update(int id, Shift shift)
    {
        var response = await _client.PutAsJsonAsync($"api/shifts/{id}", shift);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id)
    {
        var response = await _client.DeleteAsync($"api/shifts/{id}");
        response.EnsureSuccessStatusCode();
    }

    private readonly CultureInfo enUS = new CultureInfo("en-US");

    public bool ValidDateTime(string input)
    {
        string[] formats = { "yyyy-M-d H:mm:ss", "yyyy-M-d H:mm" };
        return DateTime.TryParseExact(input, formats, enUS, DateTimeStyles.None, out DateTime output);
    }

    public string FormatDateTime(string time)
    {
        DateTime.TryParse(time, out DateTime output);
        return output.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string FormatDuration(TimeSpan duration)
    {
        var durationString = duration.ToString();
        duration = new TimeSpan(duration.Days, duration.Hours, duration.Minutes, duration.Seconds);
        if (duration.Days > 0)
        {
            var hours = duration.Days * 24 + duration.Hours;
            var minAndSec = durationString.Substring(durationString.IndexOf(":"));
            durationString = hours + minAndSec;
        }
        return durationString;
    }
}