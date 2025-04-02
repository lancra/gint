using Gint.Core.External;

namespace Gint.Core.Operations;

/// <summary>
/// Represents the result of an operation execution.
/// </summary>
public class OperationResult
{
    private OperationResult(
        OperationContext context,
        IReadOnlyCollection<OperationResult> innerResults,
        bool succeeded,
        bool wrote,
        bool exit)
    {
        Context = context;
        InnerResults = innerResults;
        Succeeded = succeeded;
        Wrote = wrote;
        Exit = exit;
    }

    /// <summary>
    /// Gets the context for the operation that was executed.
    /// </summary>
    public OperationContext Context { get; }

    /// <summary>
    /// Gets the operation execution results received as part of this operation execution.
    /// </summary>
    public IReadOnlyCollection<OperationResult> InnerResults { get; }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// Gets a value indicating whether the operation wrote changes.
    /// </summary>
    public bool Wrote { get; }

    /// <summary>
    /// Gets a value indicating whether the application should exit.
    /// </summary>
    public bool Exit { get; }

    internal static OperationResult Command(OperationContext context, GitCommandResult commandResult)
    {
        var wrote = commandResult.Succeeded && context.Runspec.Descriptor.Kind == OperationKind.Write;
        var exit = wrote && context.Scope == OperationScope.File;

        return new(context, [], commandResult.Succeeded, wrote, exit);
    }

    internal static OperationResult Derived(OperationContext context, IReadOnlyCollection<OperationResult> results)
    {
        var succeeded = results.All(result => result.Succeeded);
        var wrote = results.Any(result => result.Succeeded && result.Wrote);
        var exit = (wrote && context.Scope == OperationScope.File) ||
            results.Concat(results.SelectMany(result => result.InnerResults))
                .Any(result => result.Context.Runspec.Descriptor == OperationDescriptor.Quit);

        return new(context, results, succeeded, wrote, exit);
    }

    internal static OperationResult NoOperation(OperationContext context)
        => new(context, [], true, false, false);

    internal static OperationResult Terminal(OperationContext context)
        => new(context, [], true, false, true);
}
