using System.Net;
using System.Net.Http.Json;
using System.Text;
using ShiftsLogger.UI.Models;
using ShiftsLogger.UI.Services;

namespace ShiftsLogger.Tests.Ui;

public class ShiftApiClientTests
{
    /// <summary>Returns a canned response; can also throw to simulate a dead server.</summary>
    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) => _responder = responder;

        public static StubHandler Json(HttpStatusCode status, string json) =>
            new(_ => new HttpResponseMessage(status)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

        public static StubHandler Throws(Exception exception) =>
            new(_ => throw exception);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(_responder(request));
        }
    }

    private static ShiftApiClient Client(StubHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5273/") });

    [Fact]
    public async Task GetAllAsync_ParsesCamelCaseJson()
    {
        var client = Client(StubHandler.Json(HttpStatusCode.OK,
            """[{"id":1,"workerName":"Ana","startTime":"2026-09-14T08:00:00","endTime":"2026-09-14T16:30:00"}]"""));

        var shifts = await client.GetAllAsync();

        Assert.Single(shifts);
        Assert.Equal("Ana", shifts[0].WorkerName);
        Assert.Equal(new DateTime(2026, 9, 14, 16, 30, 0), shifts[0].EndTime);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsShiftApiException_WhenServerIsDown()
    {
        var client = Client(StubHandler.Throws(new HttpRequestException("connection refused")));

        var ex = await Assert.ThrowsAsync<ShiftApiException>(() => client.GetAllAsync());

        Assert.Contains("Could not reach the Shifts API", ex.Message);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsShiftApiException_On500WithStatusInMessage()
    {
        var client = Client(StubHandler.Json(HttpStatusCode.InternalServerError, "{}"));

        var ex = await Assert.ThrowsAsync<ShiftApiException>(() => client.GetAllAsync());

        Assert.Contains("500", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_On404()
    {
        var client = Client(StubHandler.Json(HttpStatusCode.NotFound, "{}"));

        Assert.Null(await client.GetByIdAsync(42));
    }

    [Fact]
    public async Task CreateAsync_SendsJsonBody_AndReturnsCreatedShift()
    {
        var client = Client(StubHandler.Json(HttpStatusCode.Created,
            """{"id":7,"workerName":"Ana","startTime":"2026-09-14T08:00:00","endTime":"2026-09-14T16:30:00"}"""));

        var created = await client.CreateAsync(new ShiftRequest
        {
            WorkerName = "Ana",
            StartTime = new DateTime(2026, 9, 14, 8, 0, 0),
            EndTime = new DateTime(2026, 9, 14, 16, 30, 0)
        });

        Assert.Equal(7, created.Id);
    }

    [Fact]
    public async Task CreateAsync_SurfacesApiErrorMessage_On400()
    {
        var client = Client(StubHandler.Json(HttpStatusCode.BadRequest,
            """{"error":"End time must be after the start time."}"""));

        var ex = await Assert.ThrowsAsync<ShiftApiException>(() => client.CreateAsync(new ShiftRequest()));

        Assert.Contains("End time must be after the start time.", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_On204_AndFalse_On404()
    {
        var ok = Client(StubHandler.Json(HttpStatusCode.NoContent, ""));
        var missing = Client(StubHandler.Json(HttpStatusCode.NotFound, "{}"));

        Assert.True(await ok.DeleteAsync(1));
        Assert.False(await missing.DeleteAsync(1));
    }
}
