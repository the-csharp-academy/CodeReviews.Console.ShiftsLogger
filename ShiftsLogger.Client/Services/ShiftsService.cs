using System.Net.Http.Json;

public sealed class ShiftsService : IShiftsService
{
    private readonly HttpClient _httpClient;

    public ShiftsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<ClientShift>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.GetAsync("shifts", cancellationToken),
            "load shifts",
            cancellationToken);

        return await ApiResponse.ReadJsonAsync<List<ClientShift>>(
            response,
            "load shifts",
            cancellationToken);
    }

    public async Task<ClientShift> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.GetAsync($"shifts/{id}", cancellationToken),
            $"load shift {id}",
            cancellationToken);

        return await ApiResponse.ReadJsonAsync<ClientShift>(
            response,
            $"load shift {id}",
            cancellationToken);
    }

    public async Task CreateAsync(
        ShiftWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.PostAsJsonAsync("shifts", request, cancellationToken),
            "create shift",
            cancellationToken);
    }

    public async Task UpdateAsync(
        int id,
        ShiftWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.PutAsJsonAsync($"shifts/{id}", request, cancellationToken),
            $"update shift {id}",
            cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.DeleteAsync($"shifts/{id}", cancellationToken),
            $"delete shift {id}",
            cancellationToken);
    }
}
