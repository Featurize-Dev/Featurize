using Featurize.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Features;
public sealed class AddHttpClient : IConfigureFeature
{
    public void Configure(ConsoleApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("ConsoleApp", client => client.BaseAddress = new Uri("http://www.google.com"));

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ConsoleApp"));
    }
}
