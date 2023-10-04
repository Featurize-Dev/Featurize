namespace Featurize.Tests;
public partial class Get
{
    [Test]
    public void Should_configure_options()
    {
        var features = new FeatureCollection();

        features.AddWithOptions<FeatureWithConfigurableOptions, FeatureOptions>( x=>
        {
            x.Items.Add("First");
        });
        features.Add(new ConfigureOptionsFeature());
        features.Add(new ConfigureOptionsFeature1());

        var feature = features.Get<FeatureWithConfigurableOptions>();

        feature.Should().NotBeNull();
        feature!.Options.Items.Should().HaveCount(3);

    }
}

public partial class GetEnumerator
{

    [Test]
    public void Should_Configure_Options()
    {
        var features = new FeatureCollection();
        var feature = FeatureWithConfigurableOptions.Create(new FeatureOptions());
        var config = new ConfigureOptionsFeature();

        features.Add(feature);
        features.Add(config);
        //features.Add(new ConfigureOptionsFeature1());

        _ = features.GetEnumerator();
        _ = features.GetEnumerator();

        config.IsCalled.Should().BeTrue();
        feature.Options.Items.Should().HaveCount(1);
    }
}



public class ConfigureOptionsFeature : IConfigureOptions<FeatureOptions>
{
    public bool IsCalled = false;
    public void Configure(FeatureOptions options)
    {
        IsCalled= true;
        options.Items.Add(GetType().Name);
    }
}

public class ConfigureOptionsFeature1 : IConfigureOptions<FeatureOptions>
{
    public bool IsCalled = false;
    public void Configure(FeatureOptions options)
    {
        IsCalled = true;
        options.Items.Add(GetType().Name);
    }
}

public class FeatureOptions
{
    public HashSet<string> Items { get; set; } = new();
}

public class FeatureWithConfigurableOptions :
    IFeatureWithConfigurableOptions<FeatureOptions>,
    IFeatureWithOptions<FeatureWithConfigurableOptions, FeatureOptions>
    
{
    public FeatureOptions Options { get; private set; }

    public static FeatureWithConfigurableOptions Create(FeatureOptions config)
    {
        return new FeatureWithConfigurableOptions() {
            Options = config
        };
    }

    public void Configure(IFeatureCollection features)
    {
        features.Configure(Options);

    }
}


