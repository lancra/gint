namespace Gint.Console.Commands.Run;

internal sealed class RunCommandOptions : ICommandOptions
{
    public required string Pathspec { get; init; }
}
