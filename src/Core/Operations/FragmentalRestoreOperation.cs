using Gint.Core.Changes;
using Gint.Core.External;

namespace Gint.Core.Operations;

internal sealed class FragmentalRestoreOperation(IGitCommand command) : IInternalOperation
{
    private readonly IGitCommand _command = command;

    public async Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        var arguments = new List<string>
        {
            "restore",
            "--patch",
        };

        if (context.Runspec.Area is not null && context.Runspec.Area == ChangeArea.Staging)
        {
            arguments.Add("--staged");
        }

        arguments.Add(context.Pathspec);

        var commandResult = await _command.Run(cancellationToken, arguments)
            .ConfigureAwait(false);

        return OperationResult.Command(context, commandResult);
    }
}
