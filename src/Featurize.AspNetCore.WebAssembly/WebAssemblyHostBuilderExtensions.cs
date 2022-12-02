using Featurize;
using Featurize.AspNetCore.WebAssembly;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting;
public static class WebAssemblyHostBuilderExtensions
{
    /// <summary>Gets the <see cref="IFeatureCollection"/>.</summary>
    [Pure]
    public static IFeatureCollection Features(this WebAssemblyHostBuilder builder)
    {
        if (builder.Services
            .FirstOrDefault(d => d.Lifetime == ServiceLifetime.Singleton &&
                d.ServiceType == typeof(IFeatureCollection))?
                .ImplementationInstance is not IFeatureCollection features)
        {
            features = new FeatureCollection();
            builder.Services.AddSingleton(features);
        }
        return features;
    }

    [Pure]
    public static WebAssemblyHost BuildWithFeatures(this WebAssemblyHostBuilder builder)
    {
        var features = builder.Features();
        foreach (var feature in features.GetConfigureFeatures())
        {
            feature.Configure(builder);
        }
        var application = builder.Build();
        foreach (var feature in features.GetUseFeatures())
        {
            feature.Use(application);
        }
        return application;
    }

    public static IEnumerable<IConfigureFeature> GetConfigureFeatures(this IFeatureCollection features)
        => features.OfType<IConfigureFeature>();

    public static IEnumerable<IUseFeature> GetUseFeatures(this IFeatureCollection features)
        => features.OfType<IUseFeature>();
}
