using Gint.Core.Changes;

namespace Gint.Core.Operations;

/// <summary>
/// Represents the context for an operation execution.
/// </summary>
/// <param name="Runspec">The specification for running an operation execution.</param>
/// <param name="Pathspec">The pathspec pattern used to limit files.</param>
/// <param name="Scope">The scope which the operation execution will target.</param>
/// <param name="Changes">The current changes.</param>
public record OperationContext(
    Runspec Runspec,
    Pathspec Pathspec,
    OperationScope Scope,
    ChangeGroup Changes)
{
}
