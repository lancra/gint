using SimpleExec;

namespace Gint.Core.External;

internal class GitCommand(ICommand command) : IGitCommand
{
    private const string Executable = "git";

    private readonly ICommand _command = command;

    public async Task<GitCommandResult> Read(CancellationToken cancellationToken, params IReadOnlyCollection<string> arguments)
    {
        try
        {
            var (output, _) = await _command.Read(Executable, arguments, cancellationToken)
                .ConfigureAwait(false);

            // Even on Windows, Git outputs Unix newlines.
            var lines = output?.Split("\n", StringSplitOptions.RemoveEmptyEntries) ?? [];
            return new(0, lines);
        }
        catch (ExitCodeException ex)
        {
            return new(ex.ExitCode, []);
        }
    }

    public async Task<GitCommandResult> Run(CancellationToken cancellationToken, params IReadOnlyCollection<string> arguments)
    {
        try
        {
            await _command.Run(Executable, arguments, cancellationToken)
                .ConfigureAwait(false);
            return new(0, []);
        }
        catch (ExitCodeException ex)
        {
            return new(ex.ExitCode, []);
        }
    }
}
