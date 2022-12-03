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

    private static IFeature Construct(ConstructorInfo ctor)
        => (IFeature)ctor.Invoke(Array.Empty<object>());

    private static ConstructorInfo? GetDefaultConstructor(Type type) =>
        type.GetConstructor(Array.Empty<Type>());

    private static bool IsFeatureType(Type type) =>
        typeof(IFeature).IsAssignableFrom(type) && !type.IsAbstract;

    
}
