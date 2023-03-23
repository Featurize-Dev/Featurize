using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.ConfigFeatures;

internal static class ConfigureFeatureWithOptions
{
    private static Type[] GetConfigInterfaces(IFeature feature)
    {
        return feature.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureFeature<,>)).ToArray();
    }

    internal static IFeatureCollection Configure(this IFeatureCollection features)
    {
        var results = features.Where(x => GetConfigInterfaces(x).Any())
            .SelectMany(x => {
                var interfaces = GetConfigInterfaces(x);
                var list = new List<IFeatureWithOptions>();
                foreach (var i in interfaces)
                {
                    var con = Activator.CreateInstance(typeof(FeatureWithOptionsRunner<,>).MakeGenericType(i.GetGenericArguments()), x);
                    if (con is IFeatureWithOptions r)
                        list.Add(r);
                }
                return list.ToArray();
            })
            .ToArray();

        foreach (var item in results)
        {
            item.ConfigureFeature(features);
        }

        return features;
    }

    internal static IFeature[] GetConfigurableFeatures(FeatureCollection features)
    {
        return features
            .Where(x => {
                var interfaces = x.GetType().GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureFeature<,>));
                return interfaces.Any();
            }).ToArray();
    }
}

internal interface IFeatureWithOptions
{
    void ConfigureFeature(IFeatureCollection features);
}

internal class FeatureWithOptionsRunner<TFeature, TOption> : IFeatureWithOptions
    where TFeature : IFeatureWithConfigurableOptions<TOption>
    where TOption : class
{
    private readonly IConfigureFeature<TFeature, TOption> _configFeature;

    public FeatureWithOptionsRunner(IConfigureFeature<TFeature, TOption> configFeature)
    {
        _configFeature = configFeature;
    }

    public void ConfigureFeature(IFeatureCollection features)
    {
        var feature = features.OfType<IFeatureWithConfigurableOptions<TOption>>().FirstOrDefault();
        if(feature != null)
        {
            _configFeature.Configure(feature.Options);
        }
    }
}
