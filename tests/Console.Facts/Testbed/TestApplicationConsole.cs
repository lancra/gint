using Gint.Console.IO;
using Spectre.Console;
using Spectre.Console.Testing;

namespace Gint.Console.Facts.Testbed;

internal sealed class TestApplicationConsole(TestConsoleInput input) : IApplicationConsole, IDisposable
{
    private readonly TestConsole _output = new();
    private readonly TestConsole _error = new();

    public TestApplicationConsole()
        : this(new())
    {
    }

    public IAnsiConsole Output => _output;

    public IReadOnlyList<string> OutputLines => GetLines(_output);

    public IAnsiConsole Error => _error;

    public IReadOnlyList<string> ErrorLines => GetLines(_error);

    public IConsoleInput Input { get; } = input;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static IReadOnlyList<string> GetLines(TestConsole console)

        // Since a TestConsole is initialized with a single empty line, it must be handled specifically for checks where no output is
        // expected.
        => console.Lines.Count == 1 && string.IsNullOrEmpty(console.Lines[0])
        ? []
        : console.Lines;

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _output.Dispose();
            _error.Dispose();
        }
    }
}
