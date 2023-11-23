using CongestionTaxCalculator.Contexts;
using CongestionTaxCalculator.Models;
using CongestionTaxCalculator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IVehicle, Motorbike>();
builder.Services.AddSingleton<IVehicle, Car>();
builder.Services.AddSingleton<ICongestionTaxCalculatorService, CongestionTaxCalculatorService>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Main"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();