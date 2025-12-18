using Gint.Console.Commands.Run;
using RootCommandBase = System.CommandLine.RootCommand;

namespace Gint.Console.Commands;

internal sealed class RootCommand : RootCommandBase
{
    public RootCommand(RunCommand runCommand)
        : base("Interactively manage changes from the current Git status.")
        => Add(runCommand);
}
