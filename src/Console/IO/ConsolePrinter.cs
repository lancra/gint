using Gint.Core.Changes;
using Gint.Core.IO;
using Gint.Core.Operations;
using Spectre.Console;

namespace Gint.Console.IO;

internal sealed class ConsolePrinter(IApplicationConsole console) : IPrinter
{
    private readonly IApplicationConsole _console = console;

    public void PrintChanges(ChangeGroup changes)
    {
        foreach (var file in changes.Files)
        {
            var markupSegments = new List<string>();
            foreach (var areaIndicator in file.Indicators.OrderBy(indicator => indicator.Area.Order))
            {
                areaIndicator.Indicator.ColorOverrides.TryGetValue(areaIndicator.Area, out var overrideColor);
                var color = overrideColor ?? areaIndicator.Area.Color;

                markupSegments.Add($"[{color.ColorName}]{areaIndicator.Indicator.Value}[/]");
            }

            markupSegments.Add($" {file.Path}");

            _console.Output.MarkupLine(string.Join(string.Empty, markupSegments));
        }
    }

    public void PrintHelp(IReadOnlyCollection<OperationDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            _console.Output.WriteLine($"{descriptor.Value} - {descriptor.Description}");
        }
    }
}
