using silvermax.shiftlogger.UI.Models;
using System.Net.Http.Json;

namespace silvermax.shiftlogger.UI;

internal class ShiftClient
{
    private readonly HttpClient _httpClient;

    public ShiftClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<Shift>> GetAllShifts() => await _httpClient.GetFromJsonAsync<List<Shift>>("api/shift")
        ?? throw new InvalidOperationException("Failed to retrieve shifts.");

    public async Task<Shift> CreateNewShift(CreateNewShiftDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/shift", dto);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Shift>()
            ?? throw new InvalidOperationException("Failed to create shift");
    }

    public async Task<string> DeleteShift(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/shift/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync()
            ?? throw new InvalidOperationException("Failed to delete shift");
    }

    public async Task<Shift> UpdateShift(Guid id, Shift updatedShift)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/shift/{id}", updatedShift);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Shift>()
            ?? throw new InvalidOperationException("Failed to update shift");
    }
}
