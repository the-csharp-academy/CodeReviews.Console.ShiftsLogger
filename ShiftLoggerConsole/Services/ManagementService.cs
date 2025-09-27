using System.Text.Json;

namespace ShiftLoggerConsole.Services;
internal class ManagementService
{
    private readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:7020/api/")
    };

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
            return default;
        }
    }
}
