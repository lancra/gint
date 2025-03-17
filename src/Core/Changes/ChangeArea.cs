using Ardalis.SmartEnum;

namespace Gint.Core.Changes;

/// <summary>
/// Represents the area which a change is represented in.
/// </summary>
public class ChangeArea : SmartEnum<ChangeArea, char>
{
    /// <summary>
    /// Specifies the file system changes.
    /// </summary>
    public static readonly ChangeArea Working = new("Working", '-', ChangeIndicatorColor.Working, 2);

    /// <summary>
    /// Specifies the changes staged for inclusion.
    /// </summary>
    public static readonly ChangeArea Staging = new("Staging", '+', ChangeIndicatorColor.Staging, 1);

    private ChangeArea(string name, char value, ChangeIndicatorColor color, int outputOrder)
        : base(name, value)
    {
        Color = color;
        Order = outputOrder;
    }

    /// <summary>
    /// Gets the default color for a status indicator within the area.
    /// </summary>
    public ChangeIndicatorColor Color { get; }

    /// <summary>
    /// Gets the order to use when representing areas.
    /// </summary>
    public int Order { get; }
}
