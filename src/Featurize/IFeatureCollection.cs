namespace Featurize;
public interface IFeatureCollection : IReadOnlyCollection<IFeature>
{
    IFeatureCollection Add<TFeature>() where TFeature : IFeature, new();
    IFeatureCollection Add(IFeature feature);
    TFeature? Get<TFeature>() where TFeature : IFeature;
}


public interface IFeature
{
    string Name => GetType().Name;
}