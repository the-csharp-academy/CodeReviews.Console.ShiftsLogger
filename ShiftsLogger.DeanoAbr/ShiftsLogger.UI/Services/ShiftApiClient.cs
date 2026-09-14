using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ShiftsLogger.UI.Models;

namespace ShiftsLogger.UI.Services;

/// <summary>
/// The only place that talks HTTP. Every call is wrapped so a dead or broken API
/// turns into a readable ShiftApiException instead of an unhandled crash.
/// </summary>
public sealed class ShiftApiClient : IShiftApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string ShiftsRoute = "api/shifts";

    private readonly HttpClient _http;

    public ShiftApiClient(HttpClient http) => _http = http;

    public async Task<List<ShiftDto>> GetAllAsync(CancellationToken ct = default)
    {
        using var response = await SendAsync(() => _http.GetAsync(ShiftsRoute, ct));
        await ThrowIfErrorAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<List<ShiftDto>>(JsonOptions, ct) ?? new List<ShiftDto>();
    }

    public async Task<ShiftDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var response = await SendAsync(() => _http.GetAsync($"{ShiftsRoute}/{id}", ct));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await ThrowIfErrorAsync(response, ct);
        return await response.Content.ReadFromJsonAsync<ShiftDto>(JsonOptions, ct);
    }

    public async Task<ShiftDto> CreateAsync(ShiftRequest request, CancellationToken ct = default)
    {
        using var response = await SendAsync(() => _http.PostAsJsonAsync(ShiftsRoute, request, JsonOptions, ct));
        await ThrowIfErrorAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<ShiftDto>(JsonOptions, ct)
               ?? throw new ShiftApiException("The API returned an empty response when creating the shift.");
    }

    public async Task<ShiftDto?> UpdateAsync(int id, ShiftRequest request, CancellationToken ct = default)
    {
        using var response = await SendAsync(() => _http.PutAsJsonAsync($"{ShiftsRoute}/{id}", request, JsonOptions, ct));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await ThrowIfErrorAsync(response, ct);

        return await response.Content.ReadFromJsonAsync<ShiftDto>(JsonOptions, ct)
               ?? throw new ShiftApiException("The API returned an empty response when updating the shift.");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        using var response = await SendAsync(() => _http.DeleteAsync($"{ShiftsRoute}/{id}", ct));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        await ThrowIfErrorAsync(response, ct);
        return true;
    }

    /// <summary>Converts transport-level failures into ShiftApiException.</summary>
    private static async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> send)
    {
        try
        {
            return await send();
        }
        catch (HttpRequestException ex)
        {
            throw new ShiftApiException(
                "Could not reach the Shifts API. Make sure it is running, then try again.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ShiftApiException("The Shifts API did not respond in time.", ex);
        }
    }

    /// <summary>Converts non-success status codes into ShiftApiException, quoting the API's message.</summary>
    private static async Task ThrowIfErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(ct);
        var detail = ExtractError(body);
        var message = $"The API returned {(int)response.StatusCode} {response.ReasonPhrase}.";

        throw new ShiftApiException(string.IsNullOrWhiteSpace(detail) ? message : $"{message} {detail}");
    }

    private static string ExtractError(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return string.Empty;
        }

        try
        {
            using var document = JsonDocument.Parse(body);

            if (document.RootElement.TryGetProperty("error", out var error))
            {
                return error.GetString() ?? string.Empty;
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? string.Empty;
            }
        }
        catch (JsonException)
        {
            // Non-JSON error body (e.g. an HTML error page) — fall back to the status line.
        }

        return string.Empty;
    }
}
