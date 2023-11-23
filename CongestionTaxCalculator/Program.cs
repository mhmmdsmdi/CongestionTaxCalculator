using CongestionTaxCalculator.Contexts;
using CongestionTaxCalculator.Models;
using CongestionTaxCalculator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IVehicle, Motorbike>();
builder.Services.AddSingleton<IVehicle, Car>();
builder.Services.AddScoped<ICongestionTaxCalculatorService, CongestionTaxCalculatorService>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Main"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();