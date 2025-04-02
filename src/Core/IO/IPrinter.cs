using Gint.Core.Changes;
using Gint.Core.Operations;

namespace Gint.Core.IO;

/// <summary>
/// Represents the printer for application output.
/// </summary>
public interface IPrinter
{
    /// <summary>
    /// Prints a group of changes.
    /// </summary>
    /// <param name="changes">The change group to print.</param>
    void PrintChanges(ChangeGroup changes);

    /// <summary>
    /// Prints help for the provided operations.
    /// </summary>
    /// <param name="descriptors">The operations to print help for.</param>
    void PrintHelp(IReadOnlyCollection<OperationDescriptor> descriptors);
}
