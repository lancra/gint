namespace Gint.Dev.Targets.Build;

internal sealed class BuildTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Build,
            "Executes the complete build process.",
            dependsOn: [BuildTargets.Test, BuildTargets.Publish]);
}
