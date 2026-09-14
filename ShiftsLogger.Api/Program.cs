using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Data;
using ShiftsLogger.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShiftsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ShiftsDatabase")));

builder.Services.AddScoped<IShiftService, ShiftService>();

var app = builder.Build();

// Swagger publishes the whole API surface, so it must not be reachable on a production host.
// `dotnet run` still gets it: Properties/launchSettings.json sets ASPNETCORE_ENVIRONMENT=Development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// Needed so ShiftsLogger.Tests can boot the real app with WebApplicationFactory<Program>.
public partial class Program { }
