using Featurize.AspNetCore.WebAssembly;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorFeatures.Client.Infrastructure.RootComponents;

public class AddRootComponents : IConfigureFeature
{
    public void Configure(WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
    }
}
