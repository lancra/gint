using Gint.Core.Changes;

namespace Gint.Core.Operations;

/// <summary>
/// Represents the arguments for filtering operations.
/// </summary>
/// <param name="Scope">The scope to filter by.</param>
/// <param name="Changes">The changes to filter by.</param>
public record OperationFilter(OperationScope Scope, ChangeGroup Changes)
{
}
