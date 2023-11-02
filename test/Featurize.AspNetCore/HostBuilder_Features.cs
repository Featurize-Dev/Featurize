using Microsoft.Extensions.Hosting;

namespace Featurize.AspNetCore.Tests;
public class HostBuilder_Features
{
    [Test]
    public void should_be_used()
    {
        var feature = new SimpleHostFeature();
        var builder = WebApplication.CreateBuilder();
        builder.Features()
            .Add(feature);

        var app = builder.BuildWithFeatures();
        
        feature.IsUsed.Should().BeTrue();

        var act = () => app.Services.GetRequiredService<SimpleHostRegistration>();

        act.Should().NotThrow<InvalidOperationException>();

    }
}

internal class SimpleHostFeature : IHostFeature
{
    public bool IsUsed { get; set; }

    public void Configure(IHostBuilder hostBuilder)
    {
        IsUsed = true;

        hostBuilder.ConfigureServices(s =>
        {
            s.AddTransient<SimpleHostRegistration>();
        });

    }
}

public class SimpleHostRegistration { }