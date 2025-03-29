using Gint.Core.IO;

namespace Gint.Core.Operations;

internal sealed class BreakOperation(IRunPrompt prompt) : IInternalOperation
{
    private readonly IRunPrompt _prompt = prompt;

    public async Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken)
    {
        var operationResults = new List<OperationResult>();
        var counter = 1;

        var actionableFiles = context.Changes.Files
            .Where(file => file.Indicators.Any(areaIndicator => areaIndicator.Indicator.Actionable))
            .ToArray();
        foreach (var file in actionableFiles)
        {
            var promptCounter = new RunPromptCounter(counter, actionableFiles.Length);
            var promptContext = new RunPromptContext(new(file.Path), OperationScope.File, promptCounter);
            var fileResults = await _prompt.Open(promptContext, cancellationToken)
                .ConfigureAwait(false);

            var operationResult = OperationResult.Derived(context, fileResults);
            operationResults.Add(operationResult);

            if (operationResult.Exit)
            {
                break;
            }

            counter++;
        }

        return OperationResult.Derived(context, operationResults);
    }
}
