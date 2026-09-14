using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ShiftsLogger.Api.Data;

namespace ShiftsLogger.Tests.Api;

/// <summary>Boots the real API in-memory, swapping the file-backed SQLite db for a private one.</summary>
public class ShiftsApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ShiftsDbContext>>();
            services.RemoveAll<ShiftsDbContext>();

            services.AddDbContext<ShiftsDbContext>(options => options.UseSqlite(_connection));
        });
    }

    /// <summary>
    /// A fresh :memory: database has no tables — nothing ran the migrations on it —
    /// so the schema has to be created once the host (and its DI container) exists.
    /// Without this every request fails with "no such table: Shifts" and the API answers 500.
    /// </summary>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using (var scope = host.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ShiftsDbContext>().Database.EnsureCreated();
        }

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
