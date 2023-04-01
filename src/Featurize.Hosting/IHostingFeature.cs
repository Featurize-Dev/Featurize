using Microsoft.Extensions.Hosting;
using System.Diagnostics.Contracts;

namespace Featurize.Hosting;
public interface IHostFeature : IFeature 
{
    void Configure(IHostBuilder host);
}

