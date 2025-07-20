namespace Gint.Dev;

internal static class EnvironmentVariables
{
    public static readonly DevEnvironmentVariable BuildConfiguration = new("BUILD_CONFIGURATION", "Release");
    public static readonly DevEnvironmentVariable LocalBuild = new("LOCAL_BUILD");
    public static readonly DevEnvironmentVariable LocalLint = new("LOCAL_LINT");
    public static readonly DevEnvironmentVariable VirtualizationPlatform = new("VIRTUALIZATION_PLATFORM", "podman");
}
