namespace Gint.Console.Commands;

/// <summary>
/// Represents the handler for a command.
/// </summary>
/// <typeparam name="TParameters">The type of parameters provided to the command.</typeparam>
internal interface ICommandHandler<in TParameters>
    where TParameters : ICommandParameters
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the exit code.
    /// </returns>
    Task<ExitCode> HandleAsync(TParameters parameters, CancellationToken cancellationToken);
}
