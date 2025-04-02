using Gint.Core.IO;

namespace Gint.Core.Operations;

internal sealed class HelpOperation(IOperationAccessor operationAccessor, IPrinter printer) : IInternalOperation
{
    private readonly IOperationAccessor _operationAccessor = operationAccessor;
    private readonly IPrinter _printer = printer;

    public Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        _printer.PrintHelp(_operationAccessor.Filter(new(context.Scope, context.Changes)));
        return Task.FromResult(OperationResult.NoOperation(context));
    }
}
