namespace Gint.Dev.Targets.Build;

internal sealed class BuildTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            TargetKeys.Build,
            "Executes the complete build process.",
            dependsOn: [TargetKeys.Test, TargetKeys.Publish]);
}
