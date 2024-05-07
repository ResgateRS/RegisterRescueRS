﻿using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Extensions;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Tools;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

JwtManager.secret = config["AuthenticationSettings:TokenSecret"]!;

try
{
    StartApplication();
}
catch (Exception ex)
{
    Console.WriteLine($"The application failed to start correctly: {ex.Message}");
}

void StartApplication()
{
    ConfigureBuilder(builder);

    ConfigureServices(builder.Services);

    Configure(builder.Build());
}

void ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "allOrigins",
                          policy =>
                          {
                              policy.AllowAnyOrigin();
                              policy.AllowAnyHeader();
                              policy.AllowAnyMethod();
                          });
    });
}

void ConfigureServices(IServiceCollection services)
{
    services.AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        o.ReportApiVersions = true;
    });
    services.AddVersionedApiExplorer(o =>
    {
        o.GroupNameFormat = "'v'VVV";
        o.SubstituteApiVersionInUrl = true;
    });
    services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<RegisterRescueRSDbContext>(optionsBuilder => optionsBuilder.UseOracle(builder.Configuration.GetConnectionString("RegisterRescueRSDbContext")));

    services.RegisterRepositoriesAndServices();
}

void Configure(WebApplication app)
{
    app.UseCors("allOrigins");

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

