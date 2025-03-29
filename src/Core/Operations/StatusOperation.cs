using Gint.Core.IO;

namespace Gint.Core.Operations;

internal sealed class StatusOperation(IPrinter printer) : IInternalOperation
{
    private readonly IPrinter _printer = printer;

    public Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        _printer.PrintChanges(context.Changes);
        return Task.FromResult(OperationResult.NoOperation(context));
    }
}
