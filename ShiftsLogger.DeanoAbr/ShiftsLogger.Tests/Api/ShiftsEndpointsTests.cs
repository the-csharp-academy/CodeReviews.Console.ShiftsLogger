using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ShiftsLogger.Api.Dtos;

namespace ShiftsLogger.Tests.Api;

public class ShiftsEndpointsTests : IClassFixture<ShiftsApiFactory>
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly ShiftsApiFactory _factory;

    public ShiftsEndpointsTests(ShiftsApiFactory factory) => _factory = factory;

    private static ShiftRequestDto Valid(string worker = "Ana") => new(
        worker,
        new DateTime(2026, 9, 14, 8, 0, 0),
        new DateTime(2026, 9, 14, 16, 30, 0));

    [Fact]
    public async Task Get_ReturnsEmptyArray_WhenNoShifts()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/shifts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<List<ShiftDto>>(Json);
        Assert.NotNull(body);
        Assert.Empty(body!);
    }

    [Fact]
    public async Task Post_Returns201_WithLocationAndBody()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/shifts", Valid("Poster"), Json);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<ShiftDto>(Json);
        Assert.NotNull(created);
        Assert.True(created!.Id > 0);

        // CreatedAtAction builds an absolute URI (http://localhost/api/Shifts/1 under TestServer);
        // the path is what matters, and route matching is case-insensitive.
        var location = response.Headers.Location;
        Assert.NotNull(location);
        Assert.Equal($"/api/shifts/{created.Id}", location!.AbsolutePath, ignoreCase: true);
    }

    [Fact]
    public async Task Post_Returns400_WhenEndIsBeforeStart()
    {
        var client = _factory.CreateClient();
        var bad = new ShiftRequestDto("Ana",
            new DateTime(2026, 9, 14, 16, 0, 0), new DateTime(2026, 9, 14, 8, 0, 0));

        var response = await client.PostAsJsonAsync("/api/shifts", bad, Json);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_Returns400_WhenWorkerNameMissing()
    {
        var client = _factory.CreateClient();
        var bad = new ShiftRequestDto("", new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 16, 0, 0));

        var response = await client.PostAsJsonAsync("/api/shifts", bad, Json);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenMissing()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/shifts/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_UpdatesShift_AndReturns200()
    {
        var client = _factory.CreateClient();
        var created = await (await client.PostAsJsonAsync("/api/shifts", Valid("Putter"), Json))
            .Content.ReadFromJsonAsync<ShiftDto>(Json);

        var response = await client.PutAsJsonAsync($"/api/shifts/{created!.Id}",
            new ShiftRequestDto("Putter", new DateTime(2026, 9, 14, 9, 0, 0), new DateTime(2026, 9, 14, 17, 0, 0)), Json);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<ShiftDto>(Json);
        Assert.Equal(new DateTime(2026, 9, 14, 17, 0, 0), updated!.EndTime);
    }

    [Fact]
    public async Task Put_Returns404_WhenMissing()
    {
        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/shifts/999999", Valid(), Json);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns204_ThenGetReturns404()
    {
        var client = _factory.CreateClient();
        var created = await (await client.PostAsJsonAsync("/api/shifts", Valid("Deleter"), Json))
            .Content.ReadFromJsonAsync<ShiftDto>(Json);

        var deleted = await client.DeleteAsync($"/api/shifts/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/shifts/{created.Id}")).StatusCode);
    }
}
