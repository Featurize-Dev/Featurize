using Featurize.AspNetCore;

namespace BlazorFeatures.Server.Infrastructure.Blazor;

public class AddBlazor : IUseFeature
{
    public void Use(WebApplication app)
    {
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.MapFallbackToFile("index.html");
    }
}
