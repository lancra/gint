using System.CommandLine;

namespace Gint.Console.Commands.Run;

internal sealed class RunCommand : Command<RunCommandOptions, RunCommandOptionsHandler>
{
    public RunCommand()
        : base("run", "Runs an interactive Git session.")
    {
        AddAlias("r");
        AddArgument(
            new Argument<string>(
                "pathspec",
                getDefaultValue: () => ".",
                description: "The pattern used to limit paths in Git commands."));
    }
}
