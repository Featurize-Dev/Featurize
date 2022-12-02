using Featurize.AspNetCore.WebAssembly;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorFeatures.Client.Infrastructure.AddHttp;

public class AddClient : IConfigureFeature
{
    public void Configure(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    }
}
