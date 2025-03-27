using System.Text;

namespace Gint.Dev;

internal static class DotnetCli
{
    private static readonly CompositeFormat ArgumentsFormat =
        CompositeFormat.Parse($"{{0}} --configuration Release --verbosity minimal --nologo{{1}}");

    public static async Task Run(string command, params string[] args)
    {
        var additionalArgumentsString = string.Empty;
        if (args.Length != 0)
        {
            additionalArgumentsString = $" {string.Join(' ', args)}";
        }

        var argumentsString = string.Format(null, ArgumentsFormat, command, additionalArgumentsString);
        await SimpleExec.Command.RunAsync("dotnet", argumentsString)
            .ConfigureAwait(false);
    }
}
