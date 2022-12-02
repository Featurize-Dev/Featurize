using BlazorFeatures.Client.Infrastructure.AddHttp;
using BlazorFeatures.Shared;
using System.Net.Http.Json;

namespace BlazorFeatures.Client.Features.Weather;

public static class GetForecast
{
    public static Task<WeatherForecast[]?> GetForecasts(this HttpClient client) 
        => client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
}
