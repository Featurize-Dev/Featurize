using BlazorFeatures.Shared;
using Featurize.AspNetCore;

namespace BlazorFeatures.Server.Features.Weather;

public class GetForecast : IUseFeature
{
    public void Use(WebApplication app)
    {
        app.MapGet("/weatherforecast", Get);
    }

    private IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
}
