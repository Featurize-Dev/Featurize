using Featurize;
using Featurize.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.Builder;

/// <summary>/// Extension methods for <see cref="WebApplicationBuilder"/>./// </summary>public static class WebApplicationBuilderExtensions
{
    /// <summary>Gets the <see cref="IFeatureCollection"/>.</summary>
    [Pure]
    public static IFeatureCollection Features(this WebApplicationBuilder application)
    {
        if (application.Services
            .FirstOrDefault(d => d.Lifetime == ServiceLifetime.Singleton &&
                d.ServiceType == typeof(IFeatureCollection))?
                .ImplementationInstance is not IFeatureCollection features)
        {
            features = new FeatureCollection();
            application.Services.AddSingleton(features);
        }
        return features;
    }

    /// <summary>    /// Builds the <see cref="WebApplication"/> with the registerd features.    /// </summary>    /// <param name="builder">The <see cref="WebApplicationBuilder"/>.</param>    /// <returns>Instance of <see cref="WebApplication"/>.</returns>    [Pure]
    public static WebApplication BuildWithFeatures(this WebApplicationBuilder builder)
    {
        var features = builder.Features();

        foreach (var feature in features.GetHostFeatures())
        {
            feature.Configure(builder.Host);
        }

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

    /// <summary>
    /// Gets all features implmenting <see cref="IHostFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IHostFeature"/>.</returns>
    [Pure]
    public static IEnumerable<IHostFeature> GetHostFeatures(this IFeatureCollection features)
        => features.OfType<IHostFeature>();
}
