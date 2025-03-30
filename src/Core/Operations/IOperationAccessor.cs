namespace Gint.Core.Operations;

/// <summary>
/// Represents the accessor for operations.
/// </summary>
public interface IOperationAccessor
{
    /// <summary>
    /// Gets the available operations.
    /// </summary>
    /// <param name="filter">The arguments used for filtering.</param>
    /// <returns>The filtered operations.</returns>
    IReadOnlyCollection<OperationDescriptor> Filter(OperationFilter filter);
}
