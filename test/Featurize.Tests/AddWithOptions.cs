namespace Featurize.WithOptions.Tests;

public class AddWithOptions
{
    [Test]
    public void adds_feature_to_collection_with_config()
    {
        var features = new FeatureCollection();
        var name = Guid.NewGuid().ToString();

        features.AddWithOptions<FeatureWithOptions, FeatureOptions>(conf =>
        {
            conf.Name = name;
        });

        features.Count.Should().Be(1);

        var feature = features.GetByName(name);
        feature.Should().NotBeNull();
        feature?.Name.Should().Be(name);
    }
}

public class FeatureWithOptions : IFeatureWithOptions<FeatureWithOptions, FeatureOptions>
{
    public string Name { get; set; }

    private FeatureWithOptions(FeatureOptions options)
    {
        Name= options.Name;
    }

    public static FeatureWithOptions Create(FeatureOptions config)
    {
        return new FeatureWithOptions(config);
    }
}

public class FeatureOptions
{
    public string Name { get; set; } = string.Empty;
}