namespace Gint.Console.IO;

/// <summary>
/// Represents the console standard input stream.
/// </summary>
internal interface IConsoleInput
{
    /// <summary>
    /// Reads the next line of characters from the standard input stream.
    /// </summary>
    /// <returns>
    /// The next line of characters from the standard input stream, or an empty string if no more lines are available.
    /// </returns>
    string ReadLine();
}
