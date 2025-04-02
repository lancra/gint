using Gint.Core.Operations;

namespace Gint.Core.Changes;

/// <summary>
/// Represents the accessor for changes in the repository.
/// </summary>
public interface IChangeAccessor
{
    /// <summary>
    /// Gets the changes for a provided pathspec.
    /// </summary>
    /// <param name="pathspec">The pathspec used to limit the files.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the current changes.</returns>
    Task<ChangeGroup> Get(Pathspec pathspec, CancellationToken cancellationToken);
}
