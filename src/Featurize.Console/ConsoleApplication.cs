using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

using static System.Console;

namespace Featurize.ConsoleApp;
public class ConsoleApplication
{
    private readonly IHost _host;

    internal ConsoleApplication(IHost host, ConsoleApplicationOptions options)
    {
        _host = host;
        Options = options;
        Logger = host.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger(Environment.ApplicationName ?? nameof(ConsoleApplication));
    }

    public IServiceProvider Services => _host.Services;
    public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
    public IHostEnvironment Environment => _host.Services.GetRequiredService<IHostEnvironment>();
    public IFeatureCollection Features => _host.Services.GetRequiredService<IFeatureCollection>();
    public ILogger Logger { get; }
    public ConsoleApplicationOptions Options { get; }

    public static ConsoleApplication Create(string[]? args = null) =>
        new ConsoleApplicationBuilder(new() { Args = args }).Build();

    public static ConsoleApplicationBuilder CreateBuilder() =>
        new(new());

    public static ConsoleApplicationBuilder CreateBuilder(string[] args) =>
        new(new() { Args = args });

    public static ConsoleApplicationBuilder CreateBuilder(ConsoleApplicationOptions options) =>
        new(options);

    public void Run()
    {
        var arg = Options.Args?.FirstOrDefault();
        var askedFeature = Features.GetRunableFeature(arg)
            ?? new Help();

        askedFeature?.Run(this).Wait();

        _host.Dispose();
    }
}

public class Help : IRunableFeature
{
    private const int _padding = 5;

    public string Name = "help";
    public string Description => "Overview of all commands available.";

    public Task Run(ConsoleApplication app)
    {
        Heading($"Usage:");
        SubItem($"{Process.GetCurrentProcess().ProcessName} [command]");
        WriteLine();
        Heading("Available Commands:");

        var runnables = app.Features.GetRunableFeatures();
        var maxLength = runnables.Max(x => x.Name.Length) + _padding;

        SubItem($"{Name.PadRight(maxLength)}{Description}");

        foreach (var feature in runnables)
        {
            SubItem($"{feature.Name.PadRight(maxLength)}{feature.Description}");
        }

        return Task.CompletedTask;
    }

    private void SubItem(string value)
    {
        string pad = string.Empty.PadLeft(_padding);
        WriteLine($"{pad}{value}");
    }

    private void Heading(string heading)
    {
        WriteLine(heading);
    }


}
