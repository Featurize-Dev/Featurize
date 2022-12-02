using Featurize.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ConsoleApp.Features;
public class UseHttp : IRunableFeature
{
    public string Description => "Uses the HttpClient configured by AddHttpClient feature";

    public async Task Run(ConsoleApplication app)
    {
        var client = app.Services.GetRequiredService<HttpClient>();

        var result = await client.GetAsync("/");
        
        Console.WriteLine($"Url: {client.BaseAddress}");
        Console.WriteLine(JsonSerializer.Serialize(result.Headers, new JsonSerializerOptions() {  WriteIndented = true }));

    }
}
