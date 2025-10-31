using Microsoft.EntityFrameworkCore;
using TecWebFest.Api.Data;

// Namespaces de repos/servicios
using TecWebFest.Api.Repositories;
using TecWebFest.Api.Repositories.Interfaces;
using TecWebFest.Api.Services;
using TecWebFest.Api.Services.Interfaces;

// OJO: algunos repos tuyos están en TecWebFest.Repositories
using TecWebFest.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Inyección de repositorios
builder.Services.AddScoped<IFestivalRepository, FestivalRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<IStageRepository, StageRepository>();


// Inyección de servicios
builder.Services.AddScoped<IFestivalService, FestivalService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IPerformanceService, PerformanceService>();

//TODO INYECCION DE DEPENDENCIAS
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
