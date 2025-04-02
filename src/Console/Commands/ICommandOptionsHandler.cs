namespace Gint.Console.Commands;

/// <summary>
/// Represents the handler for a command which accepts options.
/// </summary>
/// <typeparam name="TOptions">The type of options to allow for input.</typeparam>
internal interface ICommandOptionsHandler<in TOptions>
    where TOptions : ICommandOptions
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the exit code.
    /// </returns>
    Task<ExitCode> Handle(TOptions options, CancellationToken cancellationToken);
}
