namespace Featurize;

/// <summary>
/// Marks a Feature to Configure another feature.
/// </summary>
/// <typeparam name="TFeature"></typeparam>
/// <typeparam name="TOptions"></typeparam>
public interface IConfigureFeature<TFeature, TOptions> : IFeature
     where TFeature : IFeatureWithConfigurableOptions<TOptions>
{
    /// <summary>
    /// Method called to configure Dependent Feature.
    /// </summary>
    /// <param name="options"></param>
    void Configure(TOptions options);
}
