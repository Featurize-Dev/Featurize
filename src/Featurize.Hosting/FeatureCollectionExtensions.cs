using Featurize.Hosting;
using System.Diagnostics.Contracts;

namespace Featurize;
public static class FeatureCollectionExtensions
{

    /// <summary>
    /// Gets all features implementing <see cref="IHostFeature"/>.
    /// </summary>
    /// <param name="features">The <see cref="IFeatureCollection"/>.</param>
    /// <returns>List of <see cref="IHostFeature"/>.</returns>
    [Pure]
    public static IEnumerable<IHostFeature> GetHostFeatures(this IFeatureCollection features)
        => features.OfType<IHostFeature>();
}
