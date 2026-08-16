using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Data;
using ShiftsLogger.Interfaces;
using ShiftsLogger.MiddleWare;
using ShiftsLogger.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails( configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("resquestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ShiftsLoggerContextDB>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWorkerShiftService, WorkerShiftService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.MapControllers();

app.Run();
