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
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Connection with SqlServer

SetConfiguration();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SetConfiguration()
{
    builder.Services.AddDbContext<ApplicationDBContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    RegisterRepositories();
    RegisterServices();
    RegisterValidators();
    RegisterAutomapper();
}

void RegisterRepositories()
{
    builder.Services.AddScoped<IBaseRepository, BaseRepository>();
}

void RegisterServices()
{
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddHttpClient
}

void RegisterValidators()
{
    builder.Services.AddScoped<IValidator<Order>, OrderValidator>();
}

void RegisterAutomapper()
{
    List<Profile> mapperProfiles = new List<Profile> { new MapperProfile() }; 
    builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfiles(mapperProfiles)).CreateMapper());
}
