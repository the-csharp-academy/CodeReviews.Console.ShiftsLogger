using System.Net;

public sealed class ApiClientException : Exception
{
    public ApiClientException(
        string message,
        Exception? innerException = null,
        HttpStatusCode? statusCode = null,
        string? responseDetail = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseDetail = responseDetail;
    }

    public HttpStatusCode? StatusCode { get; }
    public string? ResponseDetail { get; }
}
