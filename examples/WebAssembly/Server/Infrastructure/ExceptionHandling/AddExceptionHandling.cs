using Featurize.AspNetCore;

namespace BlazorFeatures.Server.Infrastructure.ExceptionHandling;

public class AddExceptionHandling : IUseFeature
{
    public void Use(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
    }
}
