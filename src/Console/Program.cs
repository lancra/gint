using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Gint.Console;
using Gint.Console.Commands;
using Gint.Core;

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
        .AddConsole()
        .AddCore())
    .Build()
    .InvokeAsync(args)
    .ConfigureAwait(false);
