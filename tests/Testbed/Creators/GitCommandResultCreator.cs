using Gint.Core.External;

namespace Gint.Testbed.Creators;

internal static class GitCommandResultCreator
{
    public static GitCommandResult CreateRunSuccess()
        => new(true, 0, []);

    public static GitCommandResult CreateRunFailure()
        => new(false, 1, []);

    public static GitCommandResult CreateRun(int exitCode = 0)
        => new(exitCode, []);
}
