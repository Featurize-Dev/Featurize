using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

using static System.Console;

namespace Featurize.ConsoleApp;

/// <summary>
/// 
/// </summary>
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
    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>
    /// The services.
    /// </value>
    public IServiceProvider Services => _host.Services;
    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <value>
    /// The configuration.
    /// </value>
    public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
    /// <summary>
    /// Gets the environment.
    /// </summary>
    /// <value>
    /// The environment.
    /// </value>
    public IHostEnvironment Environment => _host.Services.GetRequiredService<IHostEnvironment>();
    /// <summary>
    /// Gets the features.
    /// </summary>
    /// <value>
    /// The features.
    /// </value>
    public IFeatureCollection Features => _host.Services.GetRequiredService<IFeatureCollection>();
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    public ILogger Logger { get; }
    /// <summary>
    /// Gets the options.
    /// </summary>
    /// <value>
    /// The options.
    /// </value>
    public ConsoleApplicationOptions Options { get; }

    /// <summary>
    /// Creates a <see cref="ConsoleApplication"/> for the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>a <see cref="ConsoleApplication"/></returns>
    public static ConsoleApplication Create(string[]? args = null) =>
        new ConsoleApplicationBuilder(new() { Args = args }).Build();

    /// <summary>
    /// Creates a <see cref="ConsoleApplicationBuilder" />.
    /// </summary>
    /// <returns>a <see cref="ConsoleApplicationBuilder"/>.</returns>
    public static ConsoleApplicationBuilder CreateBuilder() =>
        new(new());

    /// <summary>
    /// Creates the builder.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public static ConsoleApplicationBuilder CreateBuilder(string[] args) =>
        new(new() { Args = args });
    /// <summary>
    /// Creates the builder.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns></returns>
    public static ConsoleApplicationBuilder CreateBuilder(ConsoleApplicationOptions options) =>
        new(options);

    /// <summary>
    /// Runs this instance.
    /// </summary>
    public void Run()
    {
        var arg = Options.Args?.FirstOrDefault();
        var askedFeature = Features.GetRunableFeature(arg)
            ?? new Help();

        askedFeature?.Run(this).Wait();

        _host.Dispose();
    }
}

/// <summary>
/// A console help feature that returns a list of usable commands 
/// </summary>
/// <seealso cref="Featurize.ConsoleApp.IRunableFeature" />
public class Help : IRunableFeature
{
    private const int _padding = 5;

    /// <summary>
    /// The name of the feature
    /// </summary>
    public string Name => "help";

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description => "Overview of all commands available.";

    /// <summary>
    /// Runs the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns></returns>
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
