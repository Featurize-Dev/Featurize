using Featurize;

namespace Featurize.Tests;

public class Get
{
    [Test]
    public void Should_Return_Feature()
    {
        var feature = new Feature();
        var collection = new FeatureCollection
        {
            feature
        };

        var returnValue = collection.Get<Feature>();

        returnValue.Should().Be(feature);
    }
}

public class GetByName
{
    [TestCase("Feature")]
    [TestCase("feature")]
    [TestCase("feaTure")]
    [TestCase("FEATURE")]
    public void Should_Return_Feature(string variant)
    {
        var feature = new Feature();
        var collection = new FeatureCollection
        {
            feature
        };

        var returnValue = collection.GetByName(variant);

        returnValue.Should().Be(feature);
    }

    [Test]
    public void When_Name_Random_Should_Return_Feature()
    {
        var rnd = Guid.NewGuid().ToString();
        var feature = new Feature(rnd);
        var collection = new FeatureCollection
        {
            feature
        };

        var returnValue = collection.GetByName(rnd);

        returnValue.Should().Be(feature);
    }
}

public class Add
{
    [Test]
    public void Should_Add_Feature()
    {
        var collection = new FeatureCollection
        {
            new Feature()
        };

        collection.Count.Should().Be(1);
    }

    [Test]
    public void Should_Throw_On_Duplicate()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var collection = new FeatureCollection
            {
                new Feature(),
                new Feature()
            };
        });
    }

    [Test]
    public void Allow_Duplicates_With_DifferentName()
    {
        Assert.DoesNotThrow(() =>
        {
            var collection = new FeatureCollection
            {
                new Feature(Guid.NewGuid().ToString()),
                new Feature(Guid.NewGuid().ToString())
            };
        });
    }
}


class Feature : IFeature
{
    public Feature(string? name = null)
    {
        Name = name ?? GetType().Name;
    }

    public string Name { get; set; }
}

