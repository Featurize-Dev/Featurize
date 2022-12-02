namespace Featurize.ConsoleApp;
public interface IConsoleApplicationFeature : IConfigureFeature, IRunableFeature
{

}

public interface IConfigureFeature : IFeature
{
    void Configure(ConsoleApplicationBuilder builder);
}

public interface IRunableFeature : IFeature
{
    string Description { get; }
    Task Run(ConsoleApplication app);
}

public static class IFeatureCollectionExtensions
{
    public static IEnumerable<IConfigureFeature> GetConfigurableFeatures(this IFeatureCollection features)
        => features.OfType<IConfigureFeature>();

    public static IEnumerable<IRunableFeature> GetRunableFeatures(this IFeatureCollection features)
        => features.OfType<IRunableFeature>();

    public static IRunableFeature? GetRunableFeature(this IFeatureCollection features, string? name)
        => features.GetRunableFeatures().FirstOrDefault(feature => feature.Name == name);
}