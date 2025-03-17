using Ardalis.SmartEnum;

namespace Gint.Core.Operations;

/// <summary>
/// Represents the kind of execution that the operation provides.
/// </summary>
public class OperationKind : SmartEnum<OperationKind>
{
    /// <summary>
    /// Specifies that the operation is used for application control.
    /// </summary>
    public static readonly OperationKind Control = new("Control", 1);

    /// <summary>
    /// Specifies that the operation is used for reading changes.
    /// </summary>
    public static readonly OperationKind Read = new("Read", 2);

    /// <summary>
    /// Specifies that the operation is used for writing changes.
    /// </summary>
    public static readonly OperationKind Write = new("Write", 3);

    private OperationKind(string name, int value)
        : base(name, value)
    {
    }
}
