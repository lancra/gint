using System.Text;
using static SimpleExec.Command;

namespace Gint.Dev.Targets.Build;

internal sealed class CoverageTarget : ITarget
{
    private const string ConsoleAssemblyPattern = "+gint";
    private static readonly CompositeFormat AssemblyPatternFormat = CompositeFormat.Parse("{0}Gint.**{1}");

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Coverage,
            "Generates code coverage reports for test results.",
            dependsOn: [BuildTargets.Test],
            Execute);

    private static async Task Execute()
    {
        string[] assemblyFilterPatterns =
        [
            ConsoleAssemblyPattern,
            string.Format(null, AssemblyPatternFormat, "+", string.Empty),
            string.Format(null, AssemblyPatternFormat, "-", "Dev"),
            string.Format(null, AssemblyPatternFormat, "-", "Facts"),
            string.Format(null, AssemblyPatternFormat, "-", "Tests"),
        ];

        var commitId = await GitCli.GetCommitId()
            .ConfigureAwait(false);

        string[] arguments =
        [
            "reportgenerator",
            $"-assemblyFilters:{string.Join(',', assemblyFilterPatterns)}",
            $"-reports:{ArtifactPaths.TestResults}/*/*/coverage.cobertura.xml",
            $"-targetdir:{ArtifactPaths.Tests}/coverage",
            $"-tag:{commitId}",
            "-reporttypes:Html",
            "-title:\"Git Interactive\"",
        ];

        await RunAsync("dotnet", string.Join(' ', arguments))
            .ConfigureAwait(false);
    }
}
