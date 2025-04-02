using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace Gint.Console.IO;

[ExcludeFromCodeCoverage(Justification = "Contains third-party library and interface, functionality is tested via a dedicated fake.")]
internal sealed class ApplicationConsole(IAnsiConsole output, IAnsiConsole error, IConsoleInput input) : IApplicationConsole
{
    private static readonly Lazy<IApplicationConsole> _console = new(
        () => new ApplicationConsole(
            AnsiConsole.Create(
                new()
                {
                    Ansi = AnsiSupport.Detect,
                    ColorSystem = ColorSystemSupport.Detect,
                    Out = new AnsiConsoleOutput(System.Console.Out),
                }),
            AnsiConsole.Create(
                new()
                {
                    Ansi = AnsiSupport.Detect,
                    ColorSystem = ColorSystemSupport.Detect,
                    Out = new AnsiConsoleOutput(System.Console.Error),
                }),
            new SystemConsoleInput()));

    public static IApplicationConsole Console => _console.Value;

    public IAnsiConsole Output { get; } = output;

    public IAnsiConsole Error { get; } = error;

    public IConsoleInput Input { get; } = input;
}
