using Featurize;
using Featurize.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.Builder;
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

    public static IEnumerable<IConfigureFeature> GetConfigureFeatures(this IFeatureCollection features)
        => features.OfType<IConfigureFeature>();

    public static IEnumerable<IUseFeature> GetUseFeatures(this IFeatureCollection features)
        => features.OfType<IUseFeature>();
}
