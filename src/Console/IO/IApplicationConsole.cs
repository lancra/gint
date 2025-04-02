using Spectre.Console;

namespace Gint.Console.IO;

/// <summary>
/// Represents the console for the application.
/// </summary>
internal interface IApplicationConsole
{
    /// <summary>
    /// Gets the console for the standard output stream.
    /// </summary>
    IAnsiConsole Output { get; }

    /// <summary>
    /// Gets the console for the standard error stream.
    /// </summary>
    IAnsiConsole Error { get; }

    /// <summary>
    /// Gets the console for the standard input stream.
    /// </summary>
    IConsoleInput Input { get; }
}
