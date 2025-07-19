namespace Gint.Dev.Targets.Build;

internal sealed class DotnetTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            TargetKeys.Dotnet,
            "Builds the solution into a set of output binaries.",
            dependsOn: [TargetKeys.Clean],
            Execute);

    private static async Task Execute()
    {
        var arguments = new List<string>
        {
            ArtifactPaths.Solution,
        };

        if (!EnvironmentVariables.LocalBuild)
        {
            arguments.Add("/warnaserror");
        }

        await DotnetCli.Run("build", [.. arguments])
            .ConfigureAwait(false);
    }
}
