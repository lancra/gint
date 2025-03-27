namespace Gint.Dev.Targets.Build;

internal sealed class CleanTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Clean,
            "Cleans .NET build artifacts from prior executions.",
            Execute);

    private static async Task Execute()
    {
        await DotnetCli.Run("clean")
            .ConfigureAwait(false);

        if (Directory.Exists(ArtifactPaths.TestResults))
        {
            string[] removeTestResultsArguments =
            [
                "-Command Remove-Item",
                $"-Path {ArtifactPaths.TestResults}",
                "-Recurse",
                "-ErrorAction SilentlyContinue",
            ];

            await PowerShell.Run(removeTestResultsArguments)
                .ConfigureAwait(false);
        }
    }
}
