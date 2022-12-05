using Featurize;
using Featurize.AspNetCore.WebAssembly;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting;
/// <summary>
/// Extensions for <see cref="WebAssemblyHostBuilder"/>
/// </summary>
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

    /// <summary>
    /// Builds a instance <see cref="WebAssemblyHost"/> with the added features.
    /// </summary>
    /// <param name="builder">The <see cref="WebAssemblyHostBuilder"/>.</param>
    /// <returns>a <see cref="WebAssemblyHost"/>.</returns>
    [Pure]
    public static WebAssemblyHost BuildWithFeatures(this WebAssemblyHostBuilder builder)
    {
        var features = builder.Features();
        foreach (var feature in features.GetServiceCollectionFeatures())
        {
            feature.Configure(builder.Services);
        }
        var application = builder.Build();
        foreach (var feature in features.GetUseFeatures())
        {
            feature.Use(application);
        }
        return application;
    }

    /// <summary>
    /// Gets all features implementing <see cref="IUseFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IUseFeature"/>.</returns>
    [Pure]
    public static IEnumerable<IUseFeature> GetUseFeatures(this IFeatureCollection features)
        => features.OfType<IUseFeature>();
}
