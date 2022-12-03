using Featurize;
using Featurize.ConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Features.WithOptions;

public static class FeatureCollectionExtensions
{

    public static ConsoleApplicationBuilder AddWithOptions(this ConsoleApplicationBuilder builder)
    {
        builder.Features.AddWithOptions<WithOptions, WithOptionsOptions>(builder.Configuration.Bind);
        return builder;
    }
}
