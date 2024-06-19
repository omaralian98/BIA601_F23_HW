using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddRequestTimeouts();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRequestTimeouts();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.MapGet("/", () =>
"""
This is an api for https://github.com/omaralian98/BIA601_F23_HW
available endpoints: 
    /api/mode1
    /api/mode2
    /api/mode3
    /api/mode4
    /api/mode5
    /api/mode6
Post Http
Json or Form
the request gets timed-out after 3 minutes
"""
);

app.Run();
