namespace Gint.Console.Commands.Run;

internal sealed class RunCommandParameters : ICommandParameters
{
    public required string Pathspec { get; init; }
}
