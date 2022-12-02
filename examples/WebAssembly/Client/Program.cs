using Featurize;
using Featurize.AspNetCore.WebAssembly;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Features().DiscoverFeatures();

await builder.BuildWithFeatures().RunAsync();