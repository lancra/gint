using Gint.Console.IO;

namespace Gint.Console.Facts.Testbed;

internal sealed class TestConsoleInput : IConsoleInput
{
    private readonly List<string> _lines = [];
    private int _index;

    public string ReadLine()
    {
        if (_index >= _lines.Count)
        {
            return string.Empty;
        }

        var line = _lines[_index];
        _index++;
        return line;
    }

    internal TestConsoleInput Add(string line)
    {
        _lines.Add(line);
        return this;
    }
}
