using Gint.Core.Changes;
using Gint.Core.IO;
using Gint.Core.Operations;
using Spectre.Console;

namespace Gint.Console.IO;

internal sealed class ConsoleRunPrompt(
    IApplicationConsole console,
    IPrinter printer,
    IChangeAccessor changeAccessor,
    IOperationAccessor operationAccessor,
    IOperation operation)
    : IRunPrompt
{
    private readonly IApplicationConsole _console = console;
    private readonly IPrinter _printer = printer;
    private readonly IChangeAccessor _changeAccessor = changeAccessor;
    private readonly IOperationAccessor _operationAccessor = operationAccessor;
    private readonly IOperation _operation = operation;

    public async Task<IReadOnlyCollection<OperationResult>> Open(RunPromptContext context, CancellationToken cancellationToken)
    {
        var changesResult = await _changeAccessor.Get(context.Pathspec, cancellationToken)
            .ConfigureAwait(false);
        if (changesResult.Changes is null)
        {
            _console.Error.MarkupLine($"[red]{changesResult.Message}[/]");
            return [];
        }

        var changes = changesResult.Changes;
        _printer.PrintChanges(changes);

        var exit = changes.Files.Count == 0;
        var operationResults = new List<OperationResult>();
        while (!exit)
        {
            var runspec = ReadRunspec(context, changes!);
            var operationContext = new OperationContext(runspec, context.Pathspec, context.Scope, changes!);

            var operationResult = await _operation.Execute(operationContext, cancellationToken)
                .ConfigureAwait(false);
            operationResults.Add(operationResult);

            if (operationResult.Wrote)
            {
                var innerChangesResult = await _changeAccessor.Get(context.Pathspec, cancellationToken)
                    .ConfigureAwait(false);
                changes = innerChangesResult.Changes;
            }

            exit = operationResult.Exit || changes!.Files.Count == 0;
        }

        return operationResults;
    }

    private Runspec ReadRunspec(RunPromptContext context, ChangeGroup changes)
    {
        var descriptors = _operationAccessor.Filter(new(context.Scope, changes));
        var descriptorsPrompt = string.Join(',', descriptors.Select(descriptor => descriptor.Value));

        var runspec = default(Runspec?);
        while (runspec is null)
        {
            var counterDisplay = context.Counter is not null
                ? $"({context.Counter.Current}/{context.Counter.Total}) "
                : string.Empty;
            _console.Output.Write($"Perform operation {counterDisplay}[{descriptorsPrompt}]: ");

            var runspecResult = Runspec.Parse(_console.Input.ReadLine(), descriptors);
            if (!string.IsNullOrEmpty(runspecResult.Message))
            {
                _console.Error.MarkupLine($"[red]{runspecResult.Message}[/]");
            }

            runspec = runspecResult.Runspec;
        }

        return runspec;
    }
}
