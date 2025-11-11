using Microsoft.EntityFrameworkCore;
using ShiftLogger.DataAccess;
using ShiftLogger.Models;
using ShiftLogger.Service;
using ShiftLogger.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IShiftLoggerService, ShiftLoggerService>();
builder.Services.AddScoped<IShiftLoggerDataAccess, ShiftLoggerDataAcess>();
builder.Services.AddScoped<ServiceUtils>();
builder.Services.AddDbContext<ShiftLoggerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:7210") // your frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
