using static SimpleExec.Command;

namespace Gint.Dev;

internal static class GitCli
{
    public static async Task<string> GetCommitId()
    {
        var (commitId, _) = await ReadAsync("git", "rev-parse HEAD")
            .ConfigureAwait(false);
        return commitId.Replace("\n", string.Empty, StringComparison.OrdinalIgnoreCase);
    }
}
