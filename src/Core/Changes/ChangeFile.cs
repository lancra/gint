namespace Gint.Core.Changes;

/// <summary>
/// Represents the changes for a file across areas.
/// </summary>
public record ChangeFile
{
    internal ChangeFile(IReadOnlyCollection<ChangeAreaIndicator> indicators, string path)
    {
        Indicators = indicators;
        Path = path;
    }

    /// <summary>
    /// Gets the status indicators across areas.
    /// </summary>
    public IReadOnlyCollection<ChangeAreaIndicator> Indicators { get; init; }

    /// <summary>
    /// Gets the path of the file.
    /// </summary>
    public string Path { get; init; }
}
