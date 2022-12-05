using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Featurize.ConsoleApp;

/// <summary>
/// A builde for creating a <see cref="ConsoleApplication"/>.
/// </summary>
public class ConsoleApplicationBuilder
{
    private readonly HostApplicationBuilder _hostApplicationBuilder;
    private readonly IFeatureCollection _features;
    private readonly ConsoleApplicationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleApplicationBuilder"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ConsoleApplicationBuilder(ConsoleApplicationOptions options)
    {
        var configuration = new ConfigurationManager();
        _features = new FeatureCollection();
        _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = options.Args,
            ApplicationName = options.ApplicationName,
            EnvironmentName = options.EnvironmentName,
            ContentRootPath = options.ContentRootPath,
            Configuration = configuration
        });

        _hostApplicationBuilder.Services.AddSingleton(_features);
        _hostApplicationBuilder.Logging.ClearProviders();

        _options = options;
    }

    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>
    /// The services.
    /// </value>
    public IServiceCollection Services => _hostApplicationBuilder.Services;
    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <value>
    /// The configuration.
    /// </value>
    public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;
    /// <summary>
    /// Gets the features.
    /// </summary>
    /// <value>
    /// The features.
    /// </value>
    public IFeatureCollection Features => _features;
    /// <summary>
    /// Gets the logging.
    /// </summary>
    /// <value>
    /// The logging.
    /// </value>
    public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

    /// <summary>
    /// Builds a instance of <see cref="ConsoleApplication"/>.
    /// </summary>
    /// <returns></returns>
    public ConsoleApplication Build()
    {
        foreach (var feature in _features.GetServiceCollectionFeatures())
        {
            feature.Configure(this.Services);
        }

        var application = _hostApplicationBuilder.Build();

        return new ConsoleApplication(application, _options);
    }
}