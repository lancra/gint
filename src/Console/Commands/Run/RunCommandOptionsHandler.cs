using Gint.Core.IO;
using Gint.Core.Operations;

namespace Gint.Console.Commands.Run;

internal sealed class RunCommandOptionsHandler(IRunPrompt prompt) : ICommandOptionsHandler<RunCommandOptions>
{
    private readonly IRunPrompt _prompt = prompt;

    public async Task<ExitCode> Handle(RunCommandOptions options, CancellationToken cancellationToken)
    {
        var context = new RunPromptContext(new(options.Pathspec), OperationScope.All);

        var results = await _prompt.Open(context, cancellationToken)
            .ConfigureAwait(false);

        return results.All(result => result.Succeeded) ? ExitCode.Success : ExitCode.Error;
    }
}
