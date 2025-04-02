using Ardalis.SmartEnum;

namespace Gint.Core.Operations;

/// <summary>
/// Represents the scope which an operation is executed in.
/// </summary>
public class OperationScope : SmartEnum<OperationScope>
{
    /// <summary>
    /// Specifies that the operation is executed on all files.
    /// </summary>
    public static readonly OperationScope All = new("All", 0);

    /// <summary>
    /// Specifies that the operation is executed on a single file.
    /// </summary>
    public static readonly OperationScope File = new("File", 1);

    private OperationScope(string name, int value)
        : base(name, value)
    {
    }
}
