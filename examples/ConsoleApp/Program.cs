using ConsoleApp.Features.WithOptions;
using Featurize;
using Featurize.ConsoleApp;

var builder = ConsoleApplication.CreateBuilder(args);

builder.Features.DiscoverFeatures();

builder.AddWithOptions();

var app = builder.Build();

app.Run();

#if DEBUG
    Console.ReadLine();
#endif