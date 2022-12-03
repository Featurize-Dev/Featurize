using Featurize;
using Featurize.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Features.WithOptions;

internal sealed class WithOptions : IConsoleApplicationFeature, IFeatureWithOptions<WithOptions, WithOptionsOptions>
{
    public static WithOptions Create(WithOptionsOptions config) => new(config);

    private WithOptions(WithOptionsOptions options)
    {
        Options = options;
    }

    public WithOptionsOptions Options { get; }

    public string Description => "runs an feature with options";

    public void Configure(ConsoleApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DummyService>();
    }

    public Task Run(ConsoleApplication app)
    {
        var service = app.Services.GetRequiredService<DummyService>();

        service.RunConfig(Options.Filename, Options.Output);

        return Task.CompletedTask;
    }
}


public class DummyService
{
    public void RunConfig(string filename, bool output)
    {
        Console.WriteLine(filename);
        Console.WriteLine(output);

    }
}