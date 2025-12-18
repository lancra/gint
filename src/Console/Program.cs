using System.Text;
using Gint.Console;
using Gint.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RootCommand = Gint.Console.Commands.RootCommand;

using var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs)
    =>
    {
        cancellationTokenSource.Cancel();
        eventArgs.Cancel = true;
    };

Console.OutputEncoding = Encoding.UTF8;

var builderSettings = new HostApplicationBuilderSettings();
var builder = Host.CreateEmptyApplicationBuilder(builderSettings);
builder.Services.AddConsole()
    .AddCore();

var app = builder.Build();

await app.StartAsync(cancellationTokenSource.Token)
    .ConfigureAwait(false);

var rootCommand = app.Services.GetRequiredService<RootCommand>();
var exitCode = await rootCommand.Parse(args)
    .InvokeAsync(cancellationToken: cancellationTokenSource.Token)
    .ConfigureAwait(false);

await app.StopAsync(cancellationTokenSource.Token)
    .ConfigureAwait(false);

return exitCode;
