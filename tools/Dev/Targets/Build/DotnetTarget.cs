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
        var arguments = new List<string>();
        if (!EnvironmentVariables.LocalBuild.IsTruthy)
        {
            arguments.Add("/warnaserror");
        }

        await DotnetCli.Run($"build {ArtifactPaths.Solution}", [.. arguments])
            .ConfigureAwait(false);
    }
}
