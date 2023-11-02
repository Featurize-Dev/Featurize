using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Tests;
internal class ConfigurableFeature_tests
{
    [Test]
    public void Configure_Should_Be_Called()
    {
        var features = new FeatureCollection();
        var configureFeature = ConfigurableFeature.Create(new Options());
        var optionsFeature = new NewConfigureOptionsFeature();

        features.Add(configureFeature);
        features.Add(optionsFeature);

        var c = features.Get<ConfigurableFeature>()!;
        c.ConfigCalled.Should().BeTrue();
    }
}


public class ConfigurableFeature : 
    IFeatureWithConfigurableOptions<Options>
{
    public bool ConfigCalled { get;set; }

    public Options Options { get; set; }

    public ConfigurableFeature(Options options)
    {
        Options = options;
    }

    public static ConfigurableFeature Create(Options options)
    {
        return new ConfigurableFeature(options);
    }

    public void Configure(IFeatureCollection features)
    {
        ConfigCalled = true;
        features.Configure(Options);
    }
}

public class NewConfigureOptionsFeature : IConfigureOptions<Options>
{
    public void Configure(Options options)
    {
        options.Enabled = true;
    }
}

public class Options
{
    public bool Enabled { get;set; }
}