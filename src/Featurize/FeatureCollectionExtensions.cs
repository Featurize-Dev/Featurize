using System.Diagnostics.Contracts;
using System.Reflection;

namespace Featurize;
public static class FeatureCollectionExtensions
{
    public static IFeatureCollection DiscoverFeatures(this IFeatureCollection builder)
    {
        return builder.DiscoverFeatures(Assembly.GetCallingAssembly());
    }

    public static IFeatureCollection DiscoverFeatures(this IFeatureCollection features, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(x => typeof(IFeature).IsAssignableFrom(x) && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IFeature>().ToList();

            foreach (var feature in types)
            {
                features.Add(feature);
            }
        }

        return features;
    }

    [Pure]
    public static IFeature? GetByName(this IFeatureCollection features, string name)
        => features.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
}
