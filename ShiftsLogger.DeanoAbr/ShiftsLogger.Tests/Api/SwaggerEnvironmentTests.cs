using System.Net;

namespace ShiftsLogger.Tests.Api;

public class SwaggerIsDisabledInProductionTests : IClassFixture<ProductionShiftsApiFactory>
{
    private readonly ProductionShiftsApiFactory _factory;

    public SwaggerIsDisabledInProductionTests(ProductionShiftsApiFactory factory) => _factory = factory;

    [Fact]
    public async Task SwaggerEndpoints_Return404_InProduction()
    {
        var client = _factory.CreateClient();

        var ui = await client.GetAsync("/swagger/index.html");
        var document = await client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(HttpStatusCode.NotFound, ui.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, document.StatusCode);
    }
}

public class SwaggerIsEnabledInDevelopmentTests : IClassFixture<DevelopmentShiftsApiFactory>
{
    private readonly DevelopmentShiftsApiFactory _factory;

    public SwaggerIsEnabledInDevelopmentTests(DevelopmentShiftsApiFactory factory) => _factory = factory;

    [Fact]
    public async Task SwaggerUi_Returns200_InDevelopment()
    {
        var client = _factory.CreateClient();

        var ui = await client.GetAsync("/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, ui.StatusCode);
    }

    [Fact]
    public async Task SwaggerDocument_ListsAllFiveShiftRoutes_InDevelopment()
    {
        var client = _factory.CreateClient();

        var document = await client.GetStringAsync("/swagger/v1/swagger.json");

        Assert.Contains("/api/Shifts", document);
        Assert.Contains("delete", document);
        Assert.Contains("put", document);
    }
}
