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

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

// Needed so ShiftsLogger.Tests can boot the real app with WebApplicationFactory<Program>.
public partial class Program { }
