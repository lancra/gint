namespace Gint.Dev.Targets.Build;

internal sealed class DotnetTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Dotnet,
            "Builds the solution into a set of output binaries.",
            dependsOn: [BuildTargets.Clean],
            Execute);

    private static async Task Execute()
    {
        var arguments = new List<string>();

        if (!EnvironmentVariables.LocalBuild)
        {
            arguments.Add("/warnaserror");
        }

        await DotnetCli.Run("build", [.. arguments])
            .ConfigureAwait(false);
    }
}
