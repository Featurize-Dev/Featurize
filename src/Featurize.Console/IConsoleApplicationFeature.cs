namespace Featurize.ConsoleApp;

/// <summary>
/// Describes a Console application feature.
/// </summary>
/// <seealso cref="Featurize.IServiceCollectionFeature" />
/// <seealso cref="Featurize.ConsoleApp.IRunableFeature" />
public interface IConsoleApplicationFeature : IServiceCollectionFeature, IRunableFeature
{

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
    /// Gets all features implementing <see cref="IRunableFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IServiceCollectionFeature"/>.</returns>
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