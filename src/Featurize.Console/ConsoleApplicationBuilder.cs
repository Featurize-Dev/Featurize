using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Featurize.ConsoleApp;

public class ConsoleApplicationBuilder
{
    private readonly HostApplicationBuilder _hostApplicationBuilder;
    private readonly IFeatureCollection _features;
    private readonly ConsoleApplicationOptions _options;

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

    public IServiceCollection Services => _hostApplicationBuilder.Services;
    public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;
    public IFeatureCollection Features => _features;
    public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

    public ConsoleApplication Build()
    {
        foreach (var feature in _features.GetConfigurableFeatures())
        {
            feature.Configure(this);
        }

        var application = _hostApplicationBuilder.Build();

        return new ConsoleApplication(application, _options);
    }
}