using Featurize.SortFeatures;

namespace Featurize.Tests;
internal class Dependency
{
    [Test]
    public void should_be_sorted()
    {
        var inOrderCollection = new FeatureCollection
        {
            new FeatureA(),
            new FeatureB(),
            new FeatureC()
        };

        var list = inOrderCollection.ToList();

        var outOfOrderCollection = new FeatureCollection()
        {
            new FeatureC(),
            new FeatureA(),
            new FeatureB()
        };

        var list1 = outOfOrderCollection.ToList();

        list1.Should().BeEquivalentTo(list);
    }

    [Test]
    public void to_self_should_be_ignored()
    {
        var inOrderCollection = new FeatureCollection
        {
            new FeatureD(),
        };

        var l = () => inOrderCollection.ToList();
        l.Should().NotThrow().Which.Count.Should().Be(1);

    }

    [Test]
    public void dependency_not_found_should_be_throw()
    {
        var inOrderCollection = new FeatureCollection
        {
            new FeatureB(),
        };

        var l = () => inOrderCollection.ToList();
        l.Should().Throw<InvalidOperationException>()
            .WithMessage("Dependenvy '*' required for '*' has not been registered.");

    }

    [Test]
    public void Circular_dependency_should_throw()
    {
        var inOrderCollection = new FeatureCollection
        {
            new FeatureE(),
            new FeatureF(),
        };

        var l = () => inOrderCollection.ToList();
        l.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Feature: '*' has cyclic dependency.");
    }
}

internal class FeatureA : IFeature { }
internal class FeatureB : IFeature, IDependsOn<FeatureA> { }
internal class FeatureC : IFeature, IDependsOn<FeatureB> { }
internal class FeatureD : IFeature, IDependsOn<FeatureD> { }

internal class FeatureD1 : IFeature, IDependsOn<FeatureA>, IDependsOn<FeatureB> { }

internal class FeatureE : IFeature, IDependsOn<FeatureF> { }
internal class FeatureF : IFeature, IDependsOn<FeatureE> { }

