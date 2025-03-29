using Gint.Core.External;

namespace Gint.Core.Operations;

internal sealed class IntendToAddOperation(IGitCommand command) : IInternalOperation
{
    private readonly IGitCommand _command = command;

    public async Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        string[] arguments =
        [
            "add",
            "--intent-to-add",
            context.Pathspec,
        ];

        var commandResult = await _command.Run(cancellationToken, arguments)
            .ConfigureAwait(false);

        return OperationResult.Command(context, commandResult);
    }
}
