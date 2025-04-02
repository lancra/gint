namespace Gint.Core.Operations;

/// <summary>
/// Represents the result from parsing a <see cref="Operations.Runspec"/>.
/// </summary>
public class RunspecParseResult
{
    private RunspecParseResult(Runspec? runspec, string message)
    {
        Runspec = runspec;
        Message = message;
    }

    /// <summary>
    /// Gets the parsed runspec if valid, otherwise <c>null</c>.
    /// </summary>
    public Runspec? Runspec { get; }

    /// <summary>
    /// Gets the message describing the result of the parse operation.
    /// </summary>
    public string Message { get; }

    internal static RunspecParseResult Success(Runspec runspec)
        => new(runspec, string.Empty);

    internal static RunspecParseResult Error(string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(errorMessage);
        return new(default, errorMessage);
    }
}
