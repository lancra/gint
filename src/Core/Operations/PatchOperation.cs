using Gint.Core.Changes;
using Gint.Core.External;

namespace Gint.Core.Operations;

internal sealed class PatchOperation(IGitCommand command, IOperation operation) : IInternalOperation
{
    private readonly IGitCommand _command = command;
    private readonly IOperation _operation = operation;

    public async Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        var operationResults = new List<OperationResult>();
        if (context.Changes.HasIndicator(ChangeIndicator.Untracked))
        {
            var intendToAddContext = context with { Runspec = new(OperationDescriptor.IntendToAdd, default), };
            var intendToAddResult = await _operation.Execute(intendToAddContext, cancellationToken)
                .ConfigureAwait(false);
            if (!intendToAddResult.Succeeded)
            {
                return intendToAddResult;
            }

            operationResults.Add(intendToAddResult);
        }

        string[] arguments =
        [
            "add",
            "--patch",
            context.Pathspec,
        ];

        var commandResult = await _command.Run(cancellationToken, arguments)
            .ConfigureAwait(false);
        operationResults.Add(OperationResult.Command(context, commandResult));
        return OperationResult.Derived(context, operationResults);
    }
}
