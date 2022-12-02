using Featurize;
using Featurize.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Features()
    .DiscoverFeatures();

var app = builder.BuildWithFeatures();

app.Run();
