using System.Reflection;
using System.Text.Json;

namespace Gint.Dev.Targets.Build;

internal sealed class SolutionTarget : ITarget
{
    private const string SolutionName = "Gint";

    private static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            TargetKeys.Solution,
            "Generates the solution used for the build process.",
            Execute);

    private static async Task Execute()
    {
        var rootDirectory = Directory.GetCurrentDirectory();
        var solutionFilterPath = Path.Combine(rootDirectory, ArtifactPaths.Root, $"{SolutionName}.slnf");

        var assemblyName = Assembly.GetExecutingAssembly()
            .GetName();
        var projectEnumerationOptions = new EnumerationOptions
        {
            MaxRecursionDepth = 2,
            RecurseSubdirectories = true,
        };
        var projectPaths = Directory.GetFiles(rootDirectory, "*.csproj", projectEnumerationOptions)
            .Where(path => Path.GetFileNameWithoutExtension(path) != assemblyName.Name)
            .Select(path => path.Replace($"{rootDirectory}\\", string.Empty))
            .ToArray();

        var solutionRelativePath = Path.Combine("..", $"{SolutionName}.slnx");
        var solutionFilter = new SolutionFilter(new(solutionRelativePath, projectPaths));
        using var fileStream = File.OpenWrite(solutionFilterPath);
        await JsonSerializer.SerializeAsync(fileStream, solutionFilter, SerializerOptions)
            .ConfigureAwait(false);
    }

    private sealed class SolutionFilter(Solution solution)
    {
        public Solution Solution { get; } = solution;
    }

    private sealed class Solution(string path, IReadOnlyCollection<string> projects)
    {
        public string Path { get; } = path;

        public IReadOnlyCollection<string> Projects { get; } = projects;
    }
}
