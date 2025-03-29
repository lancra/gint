namespace Gint.Core.Operations;

internal sealed class IgnoreOperation : IInternalOperation
{
    public Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
        => Task.FromResult(OperationResult.Terminal(context));
}
