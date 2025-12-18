using System.CommandLine;

namespace Gint.Console.Commands.Run;

internal sealed class RunCommand : CommandBase<RunCommandParameters, RunCommandHandler>
{
    private static readonly Argument<string> PathspecArgument =
        new("pathspec")
        {
            DefaultValueFactory = _ => ".",
            Description = "The pattern used to limit paths in Git commands.",
        };

    public RunCommand(ICommandHandler<RunCommandParameters> handler)
        : base("run", "Runs an interactive Git session.", handler)
    {
        Aliases.Add("r");
        Add(PathspecArgument);
    }

    protected override RunCommandParameters GetParameters(ParseResult parseResult)
        => new()
        {
            Pathspec = parseResult.GetValue(PathspecArgument)!,
        };
}
