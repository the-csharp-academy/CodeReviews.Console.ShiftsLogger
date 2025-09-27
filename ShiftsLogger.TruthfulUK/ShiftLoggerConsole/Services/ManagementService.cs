using System.Net.Http.Json;
using System.Text.Json;

namespace ShiftLoggerConsole.Services;
internal class ManagementService
{
    private readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:7020/api/")
    };

    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
    {
        try
        {
            using var response = await _httpClient.GetAsync(endpoint, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET Request failed: {ex.Message}");
            return default;
        }
    }

    public async Task<TResult?> PostAsync<TRequest, TResult>(string endpoint, TRequest body, CancellationToken ct = default)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(endpoint, body, JsonOpts, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResult>(JsonOpts, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST Request failed: {ex.Message}");
            return default;
        }
    }

    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest body, CancellationToken ct = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(endpoint, body, JsonOpts, ct);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PUT Request failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint, string? etag = null, CancellationToken ct = default)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
            if (!string.IsNullOrWhiteSpace(etag)) request.Headers.TryAddWithoutValidation("If-Match", etag);

            using var resp = await _httpClient.SendAsync(request, ct);
            resp.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DELETE Request failed: {ex.Message}");
            return false;
        }
    }
}
