using Featurize.AspNetCore;

namespace BlazorFeatures.Server.Infrastructure.Https;

public class AddHttps : IUseFeature
{
    public void Use(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
}
