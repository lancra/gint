using System.CommandLine;

namespace Gint.Console.Commands;

internal sealed class GintRootCommand : RootCommand
{
    public GintRootCommand()
        : base("Interactively manage changes from the current Git status.")
        => AddCommand(new Run.RunCommand());
}
