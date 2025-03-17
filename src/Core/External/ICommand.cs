namespace Gint.Core.External;

/// <summary>
/// Represents an external command.
/// </summary>
internal interface ICommand
{
    /// <summary>
    /// Executes an external command that captures the output.
    /// </summary>
    /// <param name="executable">The executable to use for the execution.</param>
    /// <param name="arguments">The arguments provided to the executable.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the standard output and standard error from the
    /// execution.
    /// </returns>
    Task<(string StandardOutput, string StandardError)> Read(
        string executable,
        IReadOnlyCollection<string> arguments,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes an external command that ignores the output.
    /// </summary>
    /// <param name="executable">The executable to use for the execution.</param>
    /// <param name="arguments">The arguments provided to the executable.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task Run(string executable, IReadOnlyCollection<string> arguments, CancellationToken cancellationToken);
}
