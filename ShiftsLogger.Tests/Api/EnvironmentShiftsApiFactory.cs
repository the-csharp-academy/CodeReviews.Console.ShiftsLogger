using Microsoft.AspNetCore.Hosting;

namespace ShiftsLogger.Tests.Api;

/// <summary>
/// ShiftsApiFactory pins neither environment, so whether Swagger is registered is
/// non-deterministic. These two subclasses pin it explicitly — never guess the
/// environment in a test that depends on it.
/// </summary>
public class ProductionShiftsApiFactory : ShiftsApiFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        base.ConfigureWebHost(builder);
    }
}

public class DevelopmentShiftsApiFactory : ShiftsApiFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        base.ConfigureWebHost(builder);
    }
}
