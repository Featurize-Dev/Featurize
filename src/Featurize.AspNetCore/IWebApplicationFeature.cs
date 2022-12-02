using Featurize;
using Microsoft.AspNetCore.Builder;

namespace Featurize.AspNetCore;

public interface IWebApplicationFeature : IConfigureFeature, IUseFeature
{

}

public interface IConfigureFeature : IFeature
{
    void Configure(WebApplicationBuilder builder);
}

public interface IUseFeature : IFeature
{
    public void Use(WebApplication app);
}
