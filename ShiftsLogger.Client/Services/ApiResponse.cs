using System.Net.Http.Json;
using System.Text.Json;

internal static class ApiResponse
{
    private const int MaximumDetailLength = 240;

    public static async Task<HttpResponseMessage> SendAsync(
        Func<Task<HttpResponseMessage>> sendRequest,
        string operation,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await sendRequest();
        }
        catch (HttpRequestException exception)
        {
            throw new ApiClientException(
                $"Could not {operation}: the API could not be reached. Check that the API is running and the configured URL is correct.",
                exception);
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new ApiClientException(
                $"Could not {operation}: the API request timed out. Try again.",
                exception);
        }
        catch (JsonException exception)
        {
            var location = exception.Path is null ? "the request" : $"'{exception.Path}'";
            throw new ApiClientException(
                $"Could not {operation}: request data could not be serialized as JSON at {location}. Verify the supplied values.",
                exception);
        }

        if (response.IsSuccessStatusCode)
            return response;

        using (response)
        {
            var detail = await ReadErrorDetailAsync(response, cancellationToken);
            var status = $"{(int)response.StatusCode} {response.ReasonPhrase}";
            var message = string.IsNullOrWhiteSpace(detail)
                ? $"The API returned {status}."
                : $"The API returned {status}: {detail}";

            throw new ApiClientException(
                $"Could not {operation}. {message}",
                statusCode: response.StatusCode,
                responseDetail: detail);
        }
    }

    public static async Task<T> ReadJsonAsync<T>(
        HttpResponseMessage response,
        string operation,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await response.Content.ReadFromJsonAsync<T>(
                cancellationToken: cancellationToken);
            return result ?? throw new ApiClientException(
                $"Could not {operation}: the API returned an empty response.");
        }
        catch (JsonException exception)
        {
            var location = exception.Path is null ? "the response" : $"'{exception.Path}'";
            var line = exception.LineNumber is null
                ? string.Empty
                : $" on line {exception.LineNumber.Value + 1}";

            throw new ApiClientException(
                $"Could not {operation}: the API returned invalid JSON at {location}{line}. Check the API response format.",
                exception);
        }
        catch (HttpRequestException exception)
        {
            throw new ApiClientException(
                $"Could not {operation}: the API response was interrupted while being read. Try again.",
                exception);
        }
        catch (IOException exception)
        {
            throw new ApiClientException(
                $"Could not {operation}: the API response could not be read. Try again.",
                exception);
        }
    }

    private static async Task<string?> ReadErrorDetailAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        string body;
        try
        {
            body = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(body))
            return null;

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.ValueKind == JsonValueKind.String)
                return FormatDetail(root.GetString());

            if (root.ValueKind != JsonValueKind.Object)
                return null;

            var title = GetStringProperty(root, "title");
            var detail = GetStringProperty(root, "detail");
            var errors = GetValidationErrors(root);
            var combined = string.Join(
                " ",
                new[] { title, detail, errors }
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.Ordinal));

            return FormatDetail(combined);
        }
        catch (JsonException)
        {
            return FormatDetail(body);
        }
    }

    private static string? GetStringProperty(JsonElement root, string propertyName)
    {
        foreach (var property in root.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase)
                && property.Value.ValueKind == JsonValueKind.String)
            {
                return property.Value.GetString();
            }
        }

        return null;
    }

    private static string? GetValidationErrors(JsonElement root)
    {
        foreach (var property in root.EnumerateObject())
        {
            if (!string.Equals(property.Name, "errors", StringComparison.OrdinalIgnoreCase)
                || property.Value.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            var messages = new List<string>();
            foreach (var error in property.Value.EnumerateObject())
            {
                if (error.Value.ValueKind == JsonValueKind.Array)
                {
                    messages.AddRange(error.Value.EnumerateArray()
                        .Where(value => value.ValueKind == JsonValueKind.String)
                        .Select(value => $"{error.Name}: {value.GetString()}"));
                }
                else if (error.Value.ValueKind == JsonValueKind.String)
                {
                    messages.Add($"{error.Name}: {error.Value.GetString()}");
                }
            }

            return messages.Count == 0 ? null : string.Join("; ", messages);
        }

        return null;
    }

    private static string? FormatDetail(string? detail)
    {
        if (string.IsNullOrWhiteSpace(detail))
            return null;

        var singleLine = string.Join(
            " ",
            detail.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        if (singleLine.Length <= MaximumDetailLength)
            return singleLine;

        return $"{singleLine[..MaximumDetailLength]}...";
    }
}
