namespace Featurize.ConsoleApp;

/// <summary>
/// Options for the console application.
/// </summary>
public class ConsoleApplicationOptions
{
    /// <summary>
    /// Gets or sets the arguments.
    /// </summary>
    /// <value>
    /// The arguments.
    /// </value>
    public string[]? Args { get; set; }
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    /// <value>
    /// The name of the application.
    /// </value>
    public string? ApplicationName { get; set; }
    /// <summary>
    /// Gets or sets the name of the environment.
    /// </summary>
    /// <value>
    /// The name of the environment.
    /// </value>
    public string? EnvironmentName { get; set; }
    /// <summary>
    /// Gets or sets the content root path.
    /// </summary>
    /// <value>
    /// The content root path.
    /// </value>
    public string? ContentRootPath { get; set; }
}