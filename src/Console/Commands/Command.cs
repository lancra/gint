using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;

namespace Gint.Console.Commands;

internal abstract class Command<TOptions, THandler> : Command
    where TOptions : class, ICommandOptions
    where THandler : class, ICommandOptionsHandler<TOptions>
{
    protected Command(string name, string description)
        : base(name, description)
        => Handler = CommandHandler.Create<TOptions, IServiceProvider, CancellationToken>(HandleOptions);

    private static async Task<int> HandleOptions(
        TOptions options,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = ActivatorUtilities.CreateInstance<THandler>(serviceProvider);
        var exitCode = await handler.Handle(options, cancellationToken)
            .ConfigureAwait(false);
        return (int)exitCode;
    }
}
