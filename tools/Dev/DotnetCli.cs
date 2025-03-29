using System.Text;

namespace Gint.Dev;

internal static class DotnetCli
{
    private static readonly CompositeFormat ArgumentsFormat =
        CompositeFormat.Parse($"{{0}} --configuration {{1}} --verbosity minimal --nologo{{2}}");

    public static async Task Run(string command, params string[] args)
    {
        var additionalArgumentsString = string.Empty;
        if (args.Length != 0)
        {
            additionalArgumentsString = $" {string.Join(' ', args)}";
        }

        var configuration = EnvironmentVariables.BuildConfiguration.Value ?? "Release";
        var argumentsString = string.Format(null, ArgumentsFormat, command, configuration, additionalArgumentsString);
        await SimpleExec.Command.RunAsync("dotnet", argumentsString)
            .ConfigureAwait(false);
    }
}
