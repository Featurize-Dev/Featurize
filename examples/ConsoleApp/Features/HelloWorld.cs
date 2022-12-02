using Featurize.ConsoleApp;

namespace ConsoleApp.Features;
public class HelloWorld : IRunableFeature
{
    public string Name => "helloworld";
    public string Description => "Prints Hello World to the console.";

    public Task Run(ConsoleApplication app)
    {
        Console.WriteLine("Hello World");

        return Task.CompletedTask;
    }
}
