namespace Gint.Core.Operations;

/// <summary>
/// Provides the pattern used to limit files with changes.
/// </summary>
/// <param name="pattern">The pattern value.</param>
public class Pathspec(string pattern)
{
    /// <summary>
    /// Gets the pathspec pattern.
    /// </summary>
    public string Pattern { get; } = pattern;

    /// <summary>
    /// Converts the pathspec into a string.
    /// </summary>
    /// <param name="pathspec">The pathspec to convert.</param>
    public static implicit operator string(Pathspec pathspec) => pathspec?.ToString() ?? string.Empty;

    /// <summary>
    /// Returns a string that represents the pathspec.
    /// </summary>
    /// <returns>A string that represents the pathspec.</returns>
    public override string ToString() => Pattern;
}
