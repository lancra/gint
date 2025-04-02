using Microsoft.Extensions.DependencyInjection;

namespace Gint.Core.Operations;

internal class OperationHandler(IServiceProvider serviceProvider) : IOperation
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        var operation = _serviceProvider.GetRequiredKeyedService<IInternalOperation>(context.Runspec.Descriptor.Value);

        var result = await operation.Execute(context, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }
}
