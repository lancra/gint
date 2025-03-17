namespace Gint.Console.IO;

internal class SystemConsoleInput : IConsoleInput
{
    public string? ReadLine() => System.Console.ReadLine();
}
