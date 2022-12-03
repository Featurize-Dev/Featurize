namespace Featurize.ConsoleApp;

/// <summary>
/// Describes a Console application feature.
/// </summary>
/// <seealso cref="Featurize.ConsoleApp.IConfigureFeature" />
/// <seealso cref="Featurize.ConsoleApp.IRunableFeature" />
public interface IConsoleApplicationFeature : IConfigureFeature, IRunableFeature
{

}

/// <summary>
/// Describes a feature to register services.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IConfigureFeature : IFeature
{
    /// <summary>
    /// Allow the feature to register services
    /// </summary>
    /// <param name="builder">The builder.</param>
    void Configure(ConsoleApplicationBuilder builder);
}

/// <summary>
/// Describes a feature that can be run
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IRunableFeature : IFeature
{
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    string Description { get; }
    /// <summary>
    /// Runs the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns></returns>
    Task Run(ConsoleApplication app);
}

/// <summary>
/// Extension methods for <see cref="IFeatureCollection"/>.
/// </summary>
public static class IFeatureCollectionExtensions
{
    /// <summary>
    /// Gets all features implementing <see cref="IConfigureFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IConfigureFeature"/>.</returns>
    public static IEnumerable<IConfigureFeature> GetConfigurableFeatures(this IFeatureCollection features)
        => features.OfType<IConfigureFeature>();

    /// <summary>
    /// Gets all features implementing <see cref="IRunableFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IConfigureFeature"/>.</returns>
    public static IEnumerable<IRunableFeature> GetRunableFeatures(this IFeatureCollection features)
        => features.OfType<IRunableFeature>();

    /// <summary>
    /// Gets a <see cref="IRunableFeature"/> with a specific name.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <param name="name">The name of the <see cref="IRunableFeature"/>.</param>
    /// <returns>a  <see cref="IRunableFeature"/>.</returns>
    public static IRunableFeature? GetRunableFeature(this IFeatureCollection features, string? name)
        => features.GetRunableFeatures().FirstOrDefault(feature => feature.Name == name);
}