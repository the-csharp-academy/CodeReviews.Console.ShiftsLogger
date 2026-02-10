using System.Net.Http.Json;
using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI.Services;

public class WorkerService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<WorkerDto>?> GetAllWorkers()
    {
        var response = await _httpClient.GetFromJsonAsync<List<WorkerDto>>("http://localhost:5298/api/Worker");
        return response;
    }
    public async Task AddWorker(string name)
    { 
        await _httpClient.PostAsJsonAsync("http://localhost:5298/api/Worker", new { Name = name });
        //TODO: return IsSuccessStatusCode
        //return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteWorker(int id)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5298/api/Worker/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateWorker(int id, string name)
    {
        var response = await _httpClient.PutAsJsonAsync($"http://localhost:5298/api/Worker/{id}", new { Name = name } );
        return response.IsSuccessStatusCode;
    }
    public async Task<WorkerDetailsDTO?> GetWorkerDetails(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<WorkerDetailsDTO>($"http://localhost:5298/api/Worker/{id}/Details");
        return response;
    }
}