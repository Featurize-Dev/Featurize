using Featurize.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Featurize.AspNetCore.Tests;

public class Fluent_syntax
{
    [Test]
    public void creates_features_at_first_access()
    {
        var builder = WebApplication.CreateBuilder();
        var features = builder.Features();
        features.Should().BeSameAs(builder.Features());
    }
}

public class Adding
{
    [Test]
    public void only_for_types_not_registered_yet()
    {
        var features = WebApplication.CreateBuilder().Features().Add(new EmptyFeature());
        Action addition = () => features.Add(new EmptyFeature());
        addition.Should().Throw<ArgumentException>();
    }
}

public class Double
{
    [Test]
    public void only_for_types_not_registered_yet()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddSingleton(new Number(42))
            .TryAddScoped(_ => new Number(666));

        var app = builder.Build();

        app.Services.GetRequiredService<Number>().Should().Be(new Number(42));
    }

    record Number(int Value);
}


class EmptyFeature : IWebApplicationFeature
{
    public void Configure(WebApplicationBuilder builder) { }

    public void Use(WebApplication app) { }
}