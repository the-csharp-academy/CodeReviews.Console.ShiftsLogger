using ShiftsLoggerApi;
using ShiftsLoggerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShiftContext>();
builder.Services.AddScoped<ShiftValidation>();
builder.Services.AddScoped<ShiftService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ShiftContext>();
    if (!context.Database.CanConnect())
    {
        Console.WriteLine("Couldn't connect to database - check if your database is working");
        return;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();