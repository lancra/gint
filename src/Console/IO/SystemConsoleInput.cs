using System.Diagnostics.CodeAnalysis;

namespace Gint.Console.IO;

[ExcludeFromCodeCoverage(Justification = "Provides a simple wrapper around the system console.")]
internal class SystemConsoleInput : IConsoleInput
{
    public string ReadLine() => System.Console.ReadLine() ?? string.Empty;
}
