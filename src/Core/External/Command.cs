namespace Gint.Core.External;

internal class Command : ICommand
{
    public async Task<(string StandardOutput, string StandardError)> Read(
        string executable,
        IReadOnlyCollection<string> arguments,
        CancellationToken cancellationToken)
        => await SimpleExec.Command.ReadAsync(executable, args: arguments, cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async Task Run(string executable, IReadOnlyCollection<string> arguments, CancellationToken cancellationToken)
        => await SimpleExec.Command.RunAsync(executable, args: arguments, noEcho: true, cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
