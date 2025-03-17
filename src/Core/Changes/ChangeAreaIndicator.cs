namespace Gint.Core.Changes;

/// <summary>
/// Represents a change indicator within an individual area.
/// </summary>
public record ChangeAreaIndicator
{
    internal ChangeAreaIndicator(ChangeArea area, ChangeIndicator indicator)
    {
        Area = area;
        Indicator = indicator;
    }

    /// <summary>
    /// Gets the area that the indicator is targeting.
    /// </summary>
    public ChangeArea Area { get; init; }

    /// <summary>
    /// Gets the indicator that represents the changes.
    /// </summary>
    public ChangeIndicator Indicator { get; init; }
}
