using Gint.Console.Commands;
using Gint.Console.Commands.Run;
using Gint.Console.IO;
using Gint.Core.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Gint.Console;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddConsole(this IServiceCollection services)
        => services.AddSingleton(ApplicationConsole.Console)
        .AddScoped<IRunPrompt, ConsoleRunPrompt>()
        .AddScoped<IPrinter, ConsolePrinter>()
        .AddCommands();

    private static IServiceCollection AddCommands(this IServiceCollection services)
        => services.AddScoped<RootCommand>()
        .AddScoped<RunCommand>()
        .AddScoped<ICommandHandler<RunCommandParameters>, RunCommandHandler>();
}
