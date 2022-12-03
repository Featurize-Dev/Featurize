using Microsoft.AspNetCore.Builder;

namespace Featurize.AspNetCore;

/// <summary>
/// Describes a web application feature.
/// </summary>
/// <seealso cref="Featurize.AspNetCore.IConfigureFeature" />
/// <seealso cref="Featurize.AspNetCore.IUseFeature" />
public interface IWebApplicationFeature : IConfigureFeature, IUseFeature
{

}

/// <summary>
/// Describes a feature to register services.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IConfigureFeature : IFeature
{
    /// <summary>
    /// Allow the feature to register services
    /// </summary>
    /// <param name="builder">The builder.</param>
    void Configure(WebApplicationBuilder builder);
}

/// <summary>
/// Describes a feature to add to the request pipeline.
/// </summary>
/// <seealso cref="Featurize.IFeature" />
public interface IUseFeature : IFeature
{
    /// <summary>
    /// Allow the feature to add to the request pipeline.
    /// </summary>
    /// <param name="app">The application.</param>
    public void Use(WebApplication app);
}
