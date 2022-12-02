using Featurize;
using Featurize.ConsoleApp;

var builder = ConsoleApplication.CreateBuilder(args);

builder.Features.DiscoverFeatures();

var app = builder.Build();

app.Run();

Console.ReadLine();