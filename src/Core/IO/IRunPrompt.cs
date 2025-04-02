using Gint.Core.Operations;

namespace Gint.Core.IO;

/// <summary>
/// Represents the prompt for running a series of operations.
/// </summary>
public interface IRunPrompt
{
    /// <summary>
    /// Opens the prompt.
    /// </summary>
    /// <param name="context">The context used to customize prompt options.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the results of the executed operations.
    /// </returns>
    Task<IReadOnlyCollection<OperationResult>> Open(RunPromptContext context, CancellationToken cancellationToken);
}
