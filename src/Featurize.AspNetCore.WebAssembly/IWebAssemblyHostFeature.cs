using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Featurize.AspNetCore.WebAssembly;

/// <summary>
/// Describes a web assembly feature.
/// </summary>
/// <seealso cref="IConfigureFeature" />
/// <seealso cref="IUseFeature" />
public interface IWebAssemblyHostFeature : IConfigureFeature, IUseFeature
{

}

/// <summary>
/// Describes a feature that can manipulate the <see cref="WebAssemblyHostBuilder"/>.
/// </summary>
/// <seealso cref="IFeature" />
public interface IConfigureFeature : IFeature
{
    /// <summary>
    /// Configures the specified builder.
    /// </summary>
    /// <param name="builder">The builder.</param>
    void Configure(WebAssemblyHostBuilder builder);
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
