namespace Featurize;

/// <summary>
/// Marks as a Configurable Feature.
/// </summary>
public interface IConfigurableFeature : IFeature
{
    /// <summary>
    /// Method get calls when configuration is required.
    /// </summary>
    /// <param name="features">The feature collection.</param>
    void Configure(IFeatureCollection features);
}

/// <summary>
/// Marks a feature that configures another features options. 
/// </summary>
/// <typeparam name="TOptions">The Options of another feature.</typeparam>
public interface IConfigureOptions<TOptions> : IFeature
{
    /// <summary>
    /// Method gets called when feature requires configuration.
    /// </summary>
    /// <param name="options">The options of the feature that request configuration.</param>
    void Configure(TOptions options);
}
