using Gint.Core.Operations;

namespace Gint.Core.IO;

/// <summary>
/// Provides context for opening an <see cref="IRunPrompt"/>.
/// </summary>
/// <param name="Pathspec">The pathspec used to limit files.</param>
/// <param name="Scope">The scope in which the operation is executed.</param>
/// <param name="Counter">The counter used to display file progress.</param>
public record RunPromptContext(
    Pathspec Pathspec,
    OperationScope Scope,
    RunPromptCounter? Counter = default)
{
}
