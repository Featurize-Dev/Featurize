namespace Featurize.ConfigFeatures;
internal static class ConfigureFeatureExtension
{
    internal static IFeatureCollection Configure(this IFeatureCollection features)
    {
        foreach (var feature in features.OfType<IConfigurableFeature>())
        {
            feature.Configure(features);
        }
        return features;
    }
}
