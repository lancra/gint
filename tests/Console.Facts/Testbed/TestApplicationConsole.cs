using Gint.Console.IO;
using Spectre.Console;
using Spectre.Console.Testing;

namespace Gint.Console.Facts.Testbed;

internal sealed class TestApplicationConsole : IApplicationConsole, IDisposable
{
    private const int Width = 200;

    private readonly TestConsole _output = new();
    private readonly TestConsole _error = new();
    private readonly TestConsoleInput _input = new();

    public TestApplicationConsole()
    {
        _output.Profile.Width = Width;
        _error.Profile.Width = Width;
    }

    public IAnsiConsole Output => _output;

    public IReadOnlyList<string> OutputLines => GetLines(_output);

    public IAnsiConsole Error => _error;

    public IReadOnlyList<string> ErrorLines => GetLines(_error);

    public IConsoleInput Input => _input;

    public void AddInputLines(params IReadOnlyCollection<string> lines)
    {
        foreach (var line in lines)
        {
            _input.Add(line);
        }
    }

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
