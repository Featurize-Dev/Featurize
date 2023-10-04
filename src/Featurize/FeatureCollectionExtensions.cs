using System.Diagnostics.Contracts;
using System.Reflection;

namespace Featurize;
/// <summary>
/// Extension methods for <see cref="IFeatureCollection"/>
/// </summary>
public static class FeatureCollectionExtensions
{
    /// <summary>
    /// Searches the <see cref="IFeatureCollection"/> for a <see cref="IFeature"/> that matches the name.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/> to search.</param>
    /// <param name="name">The name of the <see cref="IFeature"/> to search for.</param>
    /// <returns>The <see cref="IFeature"/> that matches the name.</returns>
    [Pure]
    public static IFeature? GetByName(this IFeatureCollection features, string name)
        => features.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    /// <summary>
    /// Gets features implementing <see cref="IServiceCollectionFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns></returns>
    [Pure]
    public static IEnumerable<IServiceCollectionFeature> GetServiceCollectionFeatures(this IFeatureCollection features)
        => features.OfType<IServiceCollectionFeature>().AsEnumerable();

    /// <summary>
    /// Automaticaly scans the calling assembly for types that implement <see cref="IFeature"/> with a parameter less constructor and adds them to the collection.
    /// </summary>
    /// <param name="builder">The <see cref="IFeatureCollection"/> to add the features to.</param>
    /// <returns>Returns the modified <see cref="IFeatureCollection" />.</returns>
    public static IFeatureCollection DiscoverFeatures(this IFeatureCollection builder)
    {
        return builder.DiscoverFeatures(Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Scans the profided Assebly for types that implement <see cref="IFeature"/> with a parameter less constructor and adds them to the collection
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/> to add the features to.</param>
    /// <param name="assemblies">The <see cref="Assembly"/> to scan for types that implement <see cref="IFeature"/>.</param>
    /// <returns></returns>
    public static IFeatureCollection DiscoverFeatures(this IFeatureCollection features, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var f = assembly.GetTypes()
                .Where(IsFeatureType)
                .Select(GetDefaultConstructor)
                .OfType<ConstructorInfo>()
                .Select(Construct);

            foreach (var feature in f)
            {
                features.Add(feature);
            }
        }

        return features;
    }

    /// <summary>
    /// Gets all features implementing <see cref="IConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">The Options of the feature.</typeparam>
    /// <param name="features">The Feature Collection.</param>
    /// <returns>All features implementing <see cref="IConfigureOptions{TOptions}"/>.</returns>
    public static IEnumerable<IConfigureOptions<TOptions>> GetFeatureConfiguring<TOptions>(this IFeatureCollection features)
    {
        return features.OfType<IConfigureOptions<TOptions>>();
    }

    /// <summary>
    /// Run All Configurations 
    /// </summary>
    /// <typeparam name="TOptions">Options to Configure.</typeparam>
    /// <param name="features">The feature collection.</param>
    /// <param name="options">The options to configure.</param>
    public static void Configure<TOptions>(this IFeatureCollection features, TOptions options)
    {
        var result = features.GetFeatureConfiguring<TOptions>();
        foreach (var item in result)
        {
            item.Configure(options);
        }
        
    }

    private static IFeature Construct(ConstructorInfo ctor)
        => (IFeature)ctor.Invoke(Array.Empty<object>());

    private static ConstructorInfo? GetDefaultConstructor(Type type) =>
        type.GetConstructor(Array.Empty<Type>());

    private static bool IsFeatureType(Type type) =>
        typeof(IFeature).IsAssignableFrom(type) && !type.IsAbstract;
    
}
