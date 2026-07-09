using Microsoft.Extensions.DependencyInjection;

namespace silvermax.shiftlogger.UI;

public static class ServiceCollectionExtensions
{
    public static ServiceProvider ServiceProvider(this IServiceCollection services)
    {
        services.AddHttpClient<ShiftClient>(c => c.BaseAddress =
            new Uri("https://localhost:7203/"));
        services.AddTransient<ShiftManager>();

        return services.BuildServiceProvider();
    }
}
