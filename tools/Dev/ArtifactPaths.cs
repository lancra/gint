using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Gint.Dev;

[SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1203:Constants should appear before fields",
    Justification = "Static formats are grouped with their related constants.")]
internal static class ArtifactPaths
{
    public const string Root = "artifacts";

    public const string Executables = $"{Root}/executables";
    public static readonly CompositeFormat ExecutableFormat = CompositeFormat.Parse($"{Executables}/{{0}}/{{1}}");

    public const string Solution = $"{Root}/gint.slnf";

    public const string Tests = $"{Root}/tests";
    public const string TestResults = $"{Tests}/results";
    public static readonly CompositeFormat TestResultFormat = CompositeFormat.Parse($"{TestResults}//{{0}}");
}
