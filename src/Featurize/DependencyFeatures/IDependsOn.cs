namespace Featurize.SortFeatures;

/// <summary>
/// Marks a feature that it depends on <typeparamref name="TFeature"/>.
/// </summary>
/// <typeparam name="TFeature">The feature that it depends on.</typeparam>
public interface IDependsOn<TFeature>
    where TFeature : IFeature
{
}
