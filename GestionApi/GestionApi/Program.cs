using AutoMapper;
using FluentValidation;
using GestionApi.Config;
using GestionApi.Data;
using GestionApi.Dtos;
using GestionApi.Middleware;
using GestionApi.Models;
using GestionApi.Repository;
using GestionApi.Repository.Interfaces;
using GestionApi.Service;
using GestionApi.Service.Interfaces;
using GestionApi.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Connection with SqlServer

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

Log.Information("Starting up application");

builder.Services.AddScoped<IBaseRepository, BaseRepository>();

builder.Services.AddScoped<IValidator<OrderBaseDto>, OrderDtoValidator>();
builder.Services.AddScoped<IValidator<OrderQuery>, OrderQueryValidator>();

builder.Services.AddScoped<IOrderService, OrderService>();

List<Profile> mapperProfiles = new List<Profile> { new MapperProfile() };
builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfiles(mapperProfiles)).CreateMapper());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseMiddleware<LogRequestsMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception e)
{
    Log.Error(e, "An error occurred.");
}
finally
{
    Log.CloseAndFlush();
}
