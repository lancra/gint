namespace Gint.Dev.Targets.Build;

internal sealed class SolutionTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Solution,
            "Generates the solution used for the build process.",
            Execute);

    private static async Task Execute()
        => await PowerShell.Run("-Command scripts/create-solution-filter.ps1")
        .ConfigureAwait(false);
}
