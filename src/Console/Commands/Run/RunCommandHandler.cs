using Gint.Core.IO;
using Gint.Core.Operations;

namespace Gint.Console.Commands.Run;

internal sealed class RunCommandHandler(IRunPrompt prompt) : ICommandHandler<RunCommandParameters>
{
    private readonly IRunPrompt _prompt = prompt;

    public async Task<ExitCode> HandleAsync(RunCommandParameters options, CancellationToken cancellationToken)
    {
        var context = new RunPromptContext(new(options.Pathspec), OperationScope.All);

        await _prompt.Open(context, cancellationToken)
            .ConfigureAwait(false);

        return ExitCode.Success;
    }
}
