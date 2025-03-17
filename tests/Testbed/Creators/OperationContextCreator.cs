using Gint.Core.Changes;
using Gint.Core.Operations;

namespace Gint.Testbed.Creators;

/// <summary>
/// Provides creation of context for operation executions.
/// </summary>
public static class OperationContextCreator
{
    /// <summary>
    /// Creates an operation execution context for testing.
    /// </summary>
    /// <param name="runspec">The specification used to describe the operation to execute.</param>
    /// <param name="pathspec">THe pattern used to filter files.</param>
    /// <param name="scope">The scope in which to execute the operation.</param>
    /// <param name="changes">The changes present when executing the operation.</param>
    /// <returns>The created operation context.</returns>
    public static OperationContext Create(
        Runspec? runspec = default,
        Pathspec? pathspec = default,
        OperationScope? scope = default,
        ChangeGroup? changes = default)
        => new(
            runspec ?? new(OperationDescriptor.Add, default),
            pathspec ?? new("foo.txt"),
            scope ?? OperationScope.All,
            changes ?? ChangesCreator.CreateGroup(ChangesCreator.CreateFile()));
}
