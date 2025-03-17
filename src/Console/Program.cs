using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Gint.Console;
using Gint.Console.Commands;
using Gint.Console.IO;
using Microsoft.Extensions.DependencyInjection;

return await new CommandLineBuilder(new GintRootCommand())
    .UseVersionOption()
    .UseHelp()
    .UseEnvironmentVariableDirective()
    .UseParseDirective()
    .UseSuggestDirective()
    .RegisterWithDotnetSuggest()
    .UseTypoCorrections()
    .UseParseErrorReporting()
    .UseExceptionHandler()
    .CancelOnProcessTermination()
    .UseDependencyInjection(services => services
        .AddSingleton(ApplicationConsole.Console))
    .Build()
    .InvokeAsync(args)
    .ConfigureAwait(false);
