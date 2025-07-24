namespace Gint.Dev.Targets.Build;

internal sealed class TestTarget : ITarget
{
    private static readonly TestSuite[] Suites =
    [
        new(
            TargetKeys.TestUnit,
            "Tests individual components of the application.",
            [
                new("console", "tests/Console.Facts"),
                new("core", "tests/Core.Facts"),
            ]),
    ];

    public void Setup(Bullseye.Targets targets)
    {
        foreach (var suite in Suites)
        {
            SetupSuiteTarget(targets, suite);
        }

        targets.Add(
            TargetKeys.Test,
            "Executes automated test suites.",
            dependsOn: Suites.Select(suite => suite.Name)
                .ToArray());
    }

    private static void SetupSuiteTarget(Bullseye.Targets targets, TestSuite suite)
        => targets.Add(
            suite.Name,
            suite.Description,
            dependsOn: [TargetKeys.Dotnet],
            forEach: suite.Projects,
            Execute);

    private static async Task Execute(TestProject project)
        => await DotnetCli
        .Run(
            $"test {project.Path}",
            "--no-build",
            "--collect \"XPlat Code Coverage\"",
            "--logger trx",
            $"--results-directory {string.Format(null, ArtifactPaths.TestResultFormat, project.Name)}")
        .ConfigureAwait(false);
}
