using System.Reflection;

namespace Featurize.Tests;

public class Discovery
{
    [Test]
    public void ShouldAddFeaturs_in_assembly()
    {
        //var types = new[] { typeof(FeatureWithConstructor), typeof(FeatureWithoutConstructor) };

        var assembly = Assembly.GetExecutingAssembly();
        var exprected_features = assembly.GetTypes()
            .Where(x => typeof(IFeature).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => x.GetConstructor(Array.Empty<Type>()))
            .OfType<ConstructorInfo>()
            .Select(x => x.Invoke(Array.Empty<object>()));


        var features = new FeatureCollection().DiscoverFeatures();

        features.Count.Should().Be(exprected_features.Count());
    }
}

public class FeatureWithoutConstructor : IFeature
{

}

public class FeatureWithConstructor : IFeature
{
    public string Name { get; set; }
    public FeatureWithConstructor(string name)
    {
        Name= name;
    }
}