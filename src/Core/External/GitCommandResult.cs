namespace Gint.Core.External;

internal record GitCommandResult(bool Succeeded, int ExitCode, IReadOnlyCollection<string> Output)
{
    public GitCommandResult(int exitCode, IReadOnlyCollection<string> output)
        : this(exitCode == 0, exitCode, output)
    {
    }
}
