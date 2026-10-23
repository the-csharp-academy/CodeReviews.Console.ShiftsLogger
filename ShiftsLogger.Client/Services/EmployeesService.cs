using System.Net.Http.Json;

public sealed class EmployeesService : IEmployeesService
{
    private readonly HttpClient _httpClient;

    public EmployeesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<ClientEmployee>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.GetAsync("employees", cancellationToken),
            "load employees",
            cancellationToken);

        return await ApiResponse.ReadJsonAsync<List<ClientEmployee>>(
            response,
            "load employees",
            cancellationToken);
    }

    public async Task<ClientEmployee> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.GetAsync($"employees/{id}", cancellationToken),
            $"load employee {id}",
            cancellationToken);

        return await ApiResponse.ReadJsonAsync<ClientEmployee>(
            response,
            $"load employee {id}",
            cancellationToken);
    }

    public async Task CreateAsync(
        EmployeeWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.PostAsJsonAsync("employees", request, cancellationToken),
            "create employee",
            cancellationToken);
    }

    public async Task UpdateAsync(
        int id,
        EmployeeWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.PutAsJsonAsync($"employees/{id}", request, cancellationToken),
            $"update employee {id}",
            cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await ApiResponse.SendAsync(
            () => _httpClient.DeleteAsync($"employees/{id}", cancellationToken),
            $"delete employee {id}",
            cancellationToken);
    }
}
