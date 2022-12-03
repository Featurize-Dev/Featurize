using Featurize;
using Featurize.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="WebApplicationBuilder"/>.
/// </summary>
public static class WebApplicationBuilderExtensions
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

    /// <summary>
    /// Builds the <see cref="WebApplication"/> with the registerd features.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/>.</param>
    /// <returns>Instance of <see cref="WebApplication"/>.</returns>
    [Pure]
    public static WebApplication BuildWithFeatures(this WebApplicationBuilder builder)
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

    /// <summary>
    /// Gets all features implementing <see cref="IConfigureFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IConfigureFeature"/>.</returns>
    public static IEnumerable<IConfigureFeature> GetConfigureFeatures(this IFeatureCollection features)
        => features.OfType<IConfigureFeature>();

    /// <summary>
    /// Gets all features implementing <see cref="IUseFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IUseFeature"/>.</returns>
    public static IEnumerable<IUseFeature> GetUseFeatures(this IFeatureCollection features)
        => features.OfType<IUseFeature>();
}
