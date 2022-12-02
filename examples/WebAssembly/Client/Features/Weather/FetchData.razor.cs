using BlazorFeatures.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorFeatures.Client.Features.Weather;

public partial class FetchData
{
    private WeatherForecast[]? _forecasts;

    protected override async Task OnInitializedAsync()
    {
        _forecasts = await Http.GetForecasts();
    }
}
