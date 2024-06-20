using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var port = args.FirstOrDefault(arg => arg.StartsWith("--port="))?.Split('=')[1];

if (!string.IsNullOrEmpty(port) && int.TryParse(port, out var parsedPort))
{
    builder.WebHost.UseUrls($"http://localhost:{parsedPort}");
}

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

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRequestTimeouts();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

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
