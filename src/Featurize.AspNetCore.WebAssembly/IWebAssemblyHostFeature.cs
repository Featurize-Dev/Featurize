using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Featurize.AspNetCore.WebAssembly;

/// <summary>
/// Describes a web assembly feature.
/// </summary>
/// <seealso cref="IServiceCollectionFeature" />
/// <seealso cref="IUseFeature" />
public interface IWebAssemblyHostFeature : IServiceCollectionFeature, IUseFeature
{

}

/// <summary>
/// Describes a feature that can influence the request pipeline.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IUseFeature : IFeature
{
    /// <summary>
    /// Uses the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    void Use(WebAssemblyHost app);
}
