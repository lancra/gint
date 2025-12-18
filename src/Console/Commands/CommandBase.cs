using System.CommandLine;

namespace Gint.Console.Commands;

internal abstract class CommandBase<TParameters, THandler> : Command
    where TParameters : class, ICommandParameters
    where THandler : class, ICommandHandler<TParameters>
{
    protected CommandBase(string name, string description, ICommandHandler<TParameters> handler)
        : base(name, description)
        => SetAction(
            async (parseResult, cancellationToken) =>
            {
                var parameters = GetParameters(parseResult);
                var exitCode = await handler.HandleAsync(parameters, cancellationToken)
                    .ConfigureAwait(false);
                return (int)exitCode;
            });

    protected abstract TParameters GetParameters(ParseResult parseResult);
}
