using Microsoft.Extensions.DependencyInjection;

namespace Featurize;
/// <summary>
/// Describes a collection of <see cref="IFeature"/>.
/// </summary>
/// <seealso cref="IReadOnlyCollection{T}" />
public interface IFeatureCollection : IReadOnlyCollection<IFeature>
{
    /// <summary>
    /// Adds the <see cref="IFeature"/> to the collection.
    /// </summary>
    /// <typeparam name="TFeature">A type that implements <see cref="IFeature"/>.</typeparam>
    /// <returns>Returns the <see cref="IFeatureCollection"/>.</returns>
    IFeatureCollection Add<TFeature>() where TFeature : IFeature, new();

    /// <summary>
    /// Adds a instance of an <see cref="IFeature"/> object to the collection.
    /// </summary>
    /// <param name="feature">A object that implements the <see cref="IFeature"/> interface.</param>
    /// <returns>Returns the <see cref="IFeatureCollection"/>.</returns>
    /// <exception cref="ArgumentNullException">If the feature is null.</exception>
    /// <exception cref="ArgumentException">When an feature with the same name is already in the collection.</exception>
    IFeatureCollection Add(IFeature feature);

    /// <summary>
    /// Gets the instance from the collection of the given type.
    /// </summary>
    /// <typeparam name="TFeature">The type of the feature.</typeparam>
    /// <returns>Returns the instance from the collection matching the type</returns>
    TFeature? Get<TFeature>() where TFeature : IFeature;
}

/// <summary>
/// Descibes a feature type
/// </summary>
public interface IFeature
{
    /// <summary>
    /// The name of the feature (defaults to type name)
    /// </summary>
    string Name => GetType().Name;
}

/// <summary>
/// Describes a feature for registering services.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IServiceCollectionFeature : IFeature
{
    /// <summary>
    /// Configures the specified services.
    /// </summary>
    /// <param name="services">The services.</param>
    void Configure(IServiceCollection services);
}