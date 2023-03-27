using Featurize.ConfigFeatures;
using Featurize.SortFeatures;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Featurize;

/// <summary>
/// Defines methods to manipulate a collections of <see cref="IFeature"/>.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
public sealed class FeatureCollection : IFeatureCollection
{
    private bool _isConfigured = false;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly EqualityComparer _comparer = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly HashSet<IFeature> _features = new(_comparer);

    /// <summary>
    /// The number of <see cref="IFeature"/> in the collection.
    /// </summary>
    public int Count => _features.Count;


    /// <summary>
    /// Adds the <see cref="IFeature" /> to the collection.
    /// </summary>
    /// <typeparam name="TFeature">A type that implements <see cref="IFeature" />.</typeparam>
    /// <returns>
    /// Returns the <see cref="IFeatureCollection" />.
    /// </returns>
    public IFeatureCollection Add<TFeature>() where TFeature : IFeature, new() => Add(new TFeature());

    /// <summary>
    /// Adds a instance of an <see cref="IFeature" /> object to the collection.
    /// </summary>
    /// <param name="feature">A object that implements the <see cref="IFeature" /> interface.</param>
    /// <returns>
    /// Returns the <see cref="IFeatureCollection" />.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">feature</exception>
    /// <exception cref="System.ArgumentException">Feature of type '{feature.GetType()}' with same name '{feature.Name}' has already been added.</exception>
    public IFeatureCollection Add(IFeature feature)
    {
        if (!_features.Add(feature ?? throw new ArgumentNullException(nameof(feature))))
        {
            throw new ArgumentException($"Feature of type '{feature.GetType()}' with same name '{feature.Name}' has already been added.");
        }
        return this;
    }

    /// <summary>
    /// Gets the instance from the collection of the given type.
    /// </summary>
    /// <typeparam name="TFeature">The type of the feature.</typeparam>
    /// <returns>
    /// Returns the instance from the collection matching the type
    /// </returns>
    [Pure]
    public TFeature? Get<TFeature>() where TFeature : IFeature
        => this.OfType<TFeature>().FirstOrDefault();

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    [Pure]
    public IEnumerator<IFeature> GetEnumerator()  
    {
        if (!_isConfigured)
        {
            _isConfigured = true;
            this.Configure();
        }

        return _features.Sort((x) => DependencyResolver.GetDependencies(x, _features), true).GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class EqualityComparer : IEqualityComparer<IFeature>
    {
        [Pure]
        public bool Equals(IFeature? x, IFeature? y)
            => x is null
            ? y is null
            : x.GetType().Equals(y?.GetType()) &&
              x.Name.Equals(y?.Name);

        [Pure]
        public int GetHashCode([DisallowNull] IFeature obj) => obj.GetType().GetHashCode();
    }
}
