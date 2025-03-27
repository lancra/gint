using System.Text;

namespace Gint.Dev;

internal static class PowerShell
{
    private static readonly CompositeFormat ArgumentsFormat =
        CompositeFormat.Parse("-NoProfile {0} | Out-Null");

    public static async Task Run(params string[] args)
    {
        var additionalArgumentsString = string.Empty;
        if (args.Length > 0)
        {
            additionalArgumentsString = string.Join(' ', args);
        }

        var argumentsString = string.Format(null, ArgumentsFormat, additionalArgumentsString);
        await SimpleExec.Command.RunAsync("pwsh", argumentsString)
            .ConfigureAwait(false);
    }
}
