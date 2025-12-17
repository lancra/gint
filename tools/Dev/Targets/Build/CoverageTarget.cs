using System.Text;
using static SimpleExec.Command;

namespace Gint.Dev.Targets.Build;

internal sealed class CoverageTarget : ITarget
{
    private const string ConsoleAssemblyPattern = "+gint";
    private static readonly CompositeFormat AssemblyPatternFormat = CompositeFormat.Parse("{0}Gint.**{1}");

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            TargetKeys.Coverage,
            "Generates code coverage reports for test results.",
            dependsOn: [TargetKeys.Test],
            Execute);

    private static async Task Execute()
    {
        string[] assemblyFilterPatterns =
        [
            ConsoleAssemblyPattern,
            string.Format(null, AssemblyPatternFormat, "+", string.Empty),
            string.Format(null, AssemblyPatternFormat, "-", "Dev"),
            string.Format(null, AssemblyPatternFormat, "-", "Facts"),
            string.Format(null, AssemblyPatternFormat, "-", "Testbed"),
            string.Format(null, AssemblyPatternFormat, "-", "Tests"),
        ];

        var commitId = await GitCli.GetCommitId()
            .ConfigureAwait(false);

        string[] arguments =
        [
            "reportgenerator",
            $"-assemblyFilters:{string.Join(',', assemblyFilterPatterns)}",
            $"-reports:{ArtifactPaths.TestCoverageResults}",
            $"-targetdir:{ArtifactPaths.TestCoverageReports}",
            $"-tag:{commitId}",
            "-reporttypes:Html",
            "-title:\"Git Interactive\"",
        ];

        await RunAsync("dotnet", string.Join(' ', arguments))
            .ConfigureAwait(false);
    }
}
