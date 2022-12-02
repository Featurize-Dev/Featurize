using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Featurize;

[DebuggerDisplay("Count = {Count}")]
public sealed class FeatureCollection : IFeatureCollection
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly EqualityComparer _comparer = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly HashSet<IFeature> _features = new(_comparer);

    public int Count => _features.Count;

    public IFeatureCollection Add<TFeature>() where TFeature : IFeature, new() => Add(new TFeature());

    public IFeatureCollection Add(IFeature feature)
    {
        if (!_features.Add(feature ?? throw new ArgumentNullException(nameof(feature))))
        {
            throw new ArgumentException($"Feature of type '{feature.GetType()}' with same name '{feature.Name}' has already been added.");
        }
        return this;
    }

    [Pure]
    public TFeature? Get<TFeature>() where TFeature : IFeature
        => _features.OfType<TFeature>().FirstOrDefault();

    [Pure]
    public IEnumerator<IFeature> GetEnumerator() => _features.GetEnumerator();

    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class EqualityComparer : IEqualityComparer<IFeature>
    {
        [Pure]
        public bool Equals(IFeature? x, IFeature? y)
            => x is null
            ? y is null
            : x.GetType().Equals(y?.GetType()) &&
              x.Name == y.Name;

        [Pure]
        public int GetHashCode([DisallowNull] IFeature obj) => obj.GetType().GetHashCode();
    }
}
