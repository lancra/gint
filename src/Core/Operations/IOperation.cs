namespace Gint.Core.Operations;

/// <summary>
/// Represents an operation to perform on changes.
/// </summary>
public interface IOperation
{
    /// <summary>
    /// Executes the operation.
    /// </summary>
    /// <param name="context">The operation context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the operation result.</returns>
    Task<OperationResult> Execute(OperationContext context, CancellationToken cancellationToken);
}
