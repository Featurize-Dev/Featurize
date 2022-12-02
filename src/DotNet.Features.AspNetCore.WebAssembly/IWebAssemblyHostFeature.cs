using Featurize;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Featurize.AspNetCore.WebAssembly;

public interface IWebAssemblyHostFeature : IConfigureFeature, IUseFeature
{

}

public interface IConfigureFeature : IFeature
{
    void Configure(WebAssemblyHostBuilder builder);
}

public interface IUseFeature : IFeature
{
    void Use(WebAssemblyHost app);
}
