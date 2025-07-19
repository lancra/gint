namespace Gint.Dev;

internal static class EnvironmentVariables
{
    public static readonly DevEnvironmentVariable BuildConfiguration = new("BUILD_CONFIGURATION");
    public static readonly DevEnvironmentVariable LocalBuild = new("LOCAL_BUILD");
}
