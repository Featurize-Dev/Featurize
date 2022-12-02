using BlazorFeatures.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorFeatures.Client.Features.Weather;

public partial class FetchData
{
    [Inject]
    public HttpClient Http { get; set; }

    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await Http.GetForecasts();
    }
}
