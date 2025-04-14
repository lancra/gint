namespace Gint.Core.Changes;

/// <summary>
/// Represents the result of accessing the current changes.
/// </summary>
public class ChangeGroupResult
{
    private ChangeGroupResult(ChangeGroup? changes, string message)
    {
        Changes = changes;
        Message = message;
    }

    /// <summary>
    /// Gets the changes across all areas.
    /// </summary>
    public ChangeGroup? Changes { get; }

    /// <summary>
    /// Gets the message denoting a failure encountered when accessing changes.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Creates a result of successfully accessing changes.
    /// </summary>
    /// <param name="changes">The changes across all areas.</param>
    /// <returns>The result of accessing changes.</returns>
    public static ChangeGroupResult Success(ChangeGroup changes)
        => new(changes, string.Empty);

    /// <summary>
    /// Creates a result of unsuccessfully accessing changes.
    /// </summary>
    /// <param name="message">The message denoting the encountered failure.</param>
    /// <returns>The result of accessing changes.</returns>
    public static ChangeGroupResult Error(string message)
        => new(default, message);
}
