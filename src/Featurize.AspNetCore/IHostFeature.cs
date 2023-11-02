using Microsoft.Extensions.Hosting;

namespace Featurize.AspNetCore;

/// <summary>
/// Describes a HostBuilder feature.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IHostFeature : IFeature
{
    /// <summary>
    /// Allow the feature to configure the Host specific properties.
    /// </summary>
    /// <param name="hostBuilder">The Host builder.</param>
    void Configure(IHostBuilder hostBuilder);
}
