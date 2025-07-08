using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
    {
        var forecast = new[]
        {
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                25,
                "Warm"
            ),
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                18,
                "Cool"
            ),
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                30,
                "Hot"
            )
        };
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/ping", () => "pong")
    .WithName("Ping")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    [JsonPropertyName("temperatureF")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}