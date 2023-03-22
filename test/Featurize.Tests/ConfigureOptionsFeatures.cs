using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Tests;
public class ConfigureOptionsFeatures
{
    [Test]
    public void Should_configure_options()
    {
        var features = new FeatureCollection();

        features.AddWithOptions<FeatureWithOptions, FeatureOptions>();
        features.Add(new ConfigureOptionsFeature());
        features.Add(new ConfigureOptionsFeature1());

        var test = ConfigureInfo.GetConfigurableFeatures(features);

        var results = ConfigureInfo.IsConfigurable(features);

        foreach (var item in results)
        {
            item.Configure(features);
        }
                        
    }

    public abstract class ConfigureInfo
    {
        public static Type[] GetConfigInterfaces(IFeature feature)
        {
            return feature.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureFeature<,>)).ToArray();
        } 
            

        public static ConfigureInfo[] IsConfigurable(FeatureCollection features)
        {
            return features.Where(x => GetConfigInterfaces(x).Any())
                .SelectMany(x => {
                    var interfaces = GetConfigInterfaces(x);
                    var list = new List<ConfigureInfo>();
                    foreach (var i in interfaces)
                    {
                         list.Add((ConfigureInfo)Activator.CreateInstance(typeof(ConfigureInfo<,>).MakeGenericType(i.GetGenericArguments()), x));
                    }
                    return list.ToArray();
                })
                .ToArray();
        }

        public static bool GetConfigurableInterfaces(IFeature feature)
        {
            var interfaces = feature.GetType().GetInterfaces()
                        .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureFeature<,>));
            return interfaces.Any();
        }

        public static IFeature[] GetConfigurableFeatures(FeatureCollection features)
        {
            return features
                .Where(x => {
                    var interfaces = x.GetType().GetInterfaces()
                        .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConfigureFeature<,>));
                    return interfaces.Any();
                }).ToArray();
        }
        public abstract void Configure(FeatureCollection features);
    }
    public class ConfigureInfo<TFeature, TOption> : ConfigureInfo
        where TFeature : IFeatureWithConfigurableOptions<TOption>, IFeature
        where TOption : class
    {
        private readonly IConfigureFeature<TFeature, TOption> _configFeature;

        public ConfigureInfo(IConfigureFeature<TFeature, TOption> configFeature)
        {
            _configFeature = configFeature;
        }
       
        public override void Configure(FeatureCollection features)
        {
            var feature = features.OfType<IFeatureWithConfigurableOptions<TOption>>().FirstOrDefault();
            _configFeature.Configure(feature.Options);
        }
    }
    
}

public interface IConfigureFeature<TFeature, TOptions> : IFeature
     where TFeature : IFeatureWithConfigurableOptions<TOptions>
{
    void Configure(TOptions options);
}

public class ConfigureOptionsFeature : IConfigureFeature<FeatureWithOptions, FeatureOptions>
{
    public void Configure(FeatureOptions options)
    {
       options.Name= "test";
    }
}

public class ConfigureOptionsFeature1 : IConfigureFeature<FeatureWithOptions, FeatureOptions>
{
    public void Configure(FeatureOptions options)
    {
        options.Name = "test3";
    }
}


public interface IFeatureWithConfigurableOptions<TOptions>
{
    public TOptions Options { get; }
}


public class FeatureOptions
{
    public string Name { get; set; }
}

public class FeatureWithOptions : 
    IFeatureWithOptions<FeatureWithOptions, FeatureOptions>,
    IFeatureWithConfigurableOptions<FeatureOptions>
{
    public FeatureOptions Options { get; private set; }

    public static FeatureWithOptions Create(FeatureOptions config)
    {
        return new FeatureWithOptions() {
            Options = config
        };
    }
}
