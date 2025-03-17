namespace Gint.Core.External;

/// <summary>
/// Represents a Git command.
/// </summary>
internal interface IGitCommand
{
    /// <summary>
    /// Executes a Git command and reads the output.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <param name="arguments">The command arguments.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the command result.</returns>
    Task<GitCommandResult> Read(CancellationToken cancellationToken, params IReadOnlyCollection<string> arguments);

    /// <summary>
    /// Executes a Git command.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <param name="arguments">The command arguments.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the command result.</returns>
    Task<GitCommandResult> Run(CancellationToken cancellationToken, params IReadOnlyCollection<string> arguments);
}
