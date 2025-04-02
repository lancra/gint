using Gint.Core.External;
using Gint.Core.Operations;
using Gint.Core.Properties;

namespace Gint.Core.Changes;

internal class ChangeAccessor(IGitCommand command) : IChangeAccessor
{
    private readonly IGitCommand _command = command;

    public async Task<ChangeGroup> Get(Pathspec pathspec, CancellationToken cancellationToken)
    {
        string[] arguments =
        [
            "status",
            "--short",
            "--untracked-files",
            pathspec,
        ];

        var commandResult = await _command.Read(cancellationToken, arguments)
            .ConfigureAwait(false);
        if (!commandResult.Succeeded)
        {
            throw new InvalidOperationException(Messages.StatusAccessFailure(commandResult.ExitCode));
        }

        var files = new List<ChangeFile>();
        foreach (var line in commandResult.Output)
        {
            var stagingAreaIndicator = new ChangeAreaIndicator(ChangeArea.Staging, ChangeIndicator.FromValue(line[0]));
            var workingAreaIndicator = new ChangeAreaIndicator(ChangeArea.Working, ChangeIndicator.FromValue(line[1]));
            var path = line[3..];

            files.Add(new([stagingAreaIndicator, workingAreaIndicator], path));
        }

        var changes = new ChangeGroup(files);
        return changes;
    }
}
