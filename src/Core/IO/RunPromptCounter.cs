namespace Gint.Core.IO;

/// <summary>
/// Provides a counter used to display progress in an <see cref="IRunPrompt"/>.
/// </summary>
/// <param name="Current">The number representing the current file.</param>
/// <param name="Total">The total number of files.</param>
public record RunPromptCounter(int Current, int Total)
{
}
