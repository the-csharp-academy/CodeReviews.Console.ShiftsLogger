using System.ComponentModel;
using System.Net.Http.Json;
using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI.Services;

public class ShiftService
{
    private readonly HttpClient _httpClient = new();
    public async Task<bool> AddShift(ShiftCreationDto shiftCreationDto)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5298/api/Shift", shiftCreationDto);
        return response.IsSuccessStatusCode;
    }
}