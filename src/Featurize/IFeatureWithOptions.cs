namespace Featurize;

/// <summary>
/// Marks a feature that its options can be configured by other features
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public interface IFeatureWithConfigurableOptions<TOptions>
{
    /// <summary>
    /// The configurable options of the feature.
    /// </summary>
    public TOptions Options { get; }
}

/// <summary>
/// Defines a configurable feature.
/// </summary>
/// <typeparam name="TSelf">The type of the feature self.</typeparam>
/// <typeparam name="TOptions">The type of the configuration.</typeparam>
/// <seealso cref="Featurize.IFeature" />
public interface IFeatureWithOptions<TSelf, TOptions> : IFeature
    where TSelf : IFeature
    where TOptions : class
{
    /// <summary>
    /// Creates the specified feature with configuration.
    /// </summary>
    /// <param name="options">The configuration.</param>
    /// <returns>Returns a instance of the feature.</returns>
    static abstract TSelf Create(TOptions options);
}

/// <summary>
/// /Extension methods for Features with options
/// </summary>
public static class FeatureCollectionExtentions
{
    /// <summary>
    /// Adds a <see cref="IFeature"/> with options.
    /// </summary>
    /// <typeparam name="TFeature">The type of the feature.</typeparam>
    /// <typeparam name="TConfig">The type of the configuration.</typeparam>
    /// <param name="features">The features.</param>
    /// <param name="config">The configuration.</param>
    /// <returns>Returns <see cref="IFeatureCollection"/>.</returns>
    public static IFeatureCollection AddWithOptions<TFeature, TConfig>(this IFeatureCollection features, Action<TConfig>? config = null)
        where TFeature : IFeatureWithOptions<TFeature, TConfig>
        where TConfig : class, new()
    {
        var conf = new TConfig();
        config?.Invoke(conf);

        return features.Add(TFeature.Create(conf));
    }
}

