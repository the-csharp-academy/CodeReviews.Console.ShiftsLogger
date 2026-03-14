using Microsoft.EntityFrameworkCore;
using ShiftsAPI.Data;
using ShiftsAPI.Services;

namespace ShiftsAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ShiftContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString")));
        builder.Services.AddScoped<IShiftAPIService, ShiftAPIService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();
        app.Run();
    }
}