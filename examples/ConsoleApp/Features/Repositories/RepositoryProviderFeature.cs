using Featurize;
using Featurize.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Features.Repositories;
internal sealed class RepositoryProviderFeature : 
    IFeatureWithOptions<RepositoryProviderFeature, RepositoryProviderOptions>, 
    IConfigureFeature
{
    private RepositoryProviderFeature(RepositoryProviderOptions options) => Options = options;

    public RepositoryProviderOptions Options { get; }

    public static RepositoryProviderFeature Create(RepositoryProviderOptions config)
    {
        return new RepositoryProviderFeature(config);
    }

    public void Configure(ConsoleApplicationBuilder builder)
    {
        foreach (var item in Options)
        {
            var serviceType = typeof(IRepository<,>).MakeGenericType(item.valueType, item.keyType);
            var implementationType = item.serviceType;
            builder.Services.AddScoped(serviceType, implementationType);
        }
        
    }
}


public interface IRepository<T, TKey>
{

}

public class RepositoryProviderOptions : IEnumerable<RepositoryInfo>
{
    private readonly HashSet<RepositoryInfo> _infos = new();

    public void Add<T>(RepositoryInfo info)
    {
        _infos.Add(info);
    }
    
    public IEnumerator<RepositoryInfo> GetEnumerator() => _infos.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public record RepositoryInfo(Type serviceType, Type valueType, Type keyType);
