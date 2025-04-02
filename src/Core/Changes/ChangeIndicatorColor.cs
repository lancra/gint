using Ardalis.SmartEnum;

namespace Gint.Core.Changes;

/// <summary>
/// Represents the color of a change indicator.
/// </summary>
public class ChangeIndicatorColor : SmartEnum<ChangeIndicatorColor>
{
    /// <summary>
    /// Specifies the default color for a change indicator in the Working area.
    /// </summary>
    public static readonly ChangeIndicatorColor Working = new("Working", 1, "red");

    /// <summary>
    /// Specifies the default color for a change indicator in the Staging area.
    /// </summary>
    public static readonly ChangeIndicatorColor Staging = new("Staging", 2, "green");

    private ChangeIndicatorColor(string name, int value, string colorName)
        : base(name, value)
        => ColorName = colorName;

    /// <summary>
    /// Gets the name of the color.
    /// </summary>
    public string ColorName { get; }
}
