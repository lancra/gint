using Ardalis.SmartEnum;

namespace Gint.Core.Changes;

/// <summary>
/// Represents the indicator assigned for the changes in a file.
/// </summary>
public class ChangeIndicator : SmartEnum<ChangeIndicator, char>
{
    /// <summary>
    /// Specifies that the file is added or is intended to be added.
    /// </summary>
    public static readonly ChangeIndicator Added = new("Added", 'A');

    /// <summary>
    /// Specifies that the file is copied.
    /// </summary>
    public static readonly ChangeIndicator Copied = new("Copied", 'C');

    /// <summary>
    /// Specifies that the file is deleted.
    /// </summary>
    public static readonly ChangeIndicator Deleted = new("Deleted", 'D');

    /// <summary>
    /// Specifies that the file is ignored.
    /// </summary>
    public static readonly ChangeIndicator Ignored = new(
        "Ignored",
        '!',
        actionable: false,
        colorOverrides: new Dictionary<ChangeArea, ChangeIndicatorColor>
        {
            [ChangeArea.Staging] = ChangeIndicatorColor.Working,
        });

    /// <summary>
    /// Specifies that the file is modified.
    /// </summary>
    public static readonly ChangeIndicator Modified = new("Modified", 'M');

    /// <summary>
    /// Specifies that the file is renamed.
    /// </summary>
    public static readonly ChangeIndicator Renamed = new("Renamed", 'R');

    /// <summary>
    /// Specifies that the file type is changed.
    /// </summary>
    public static readonly ChangeIndicator TypeChanged = new("TypeChanged", 'T');

    /// <summary>
    /// Specifies that the file is modified with unresolved merge conflicts.
    /// </summary>
    public static readonly ChangeIndicator Unmerged = new(
        "Unmerged",
        'U',
        colorOverrides: new Dictionary<ChangeArea, ChangeIndicatorColor>
        {
            [ChangeArea.Staging] = ChangeIndicatorColor.Working,
        });

    /// <summary>
    /// Specifies that the file is changed in an unknown way.
    /// </summary>
    public static readonly ChangeIndicator Unknown = new(
        "Unknown",
        'X',
        actionable: false,
        colorOverrides: new Dictionary<ChangeArea, ChangeIndicatorColor>
        {
            [ChangeArea.Staging] = ChangeIndicatorColor.Working,
        });

    /// <summary>
    /// Specifies that the file is unmodified within an area.
    /// </summary>
    public static readonly ChangeIndicator Unmodified = new("Unmodified", ' ', actionable: false);

    /// <summary>
    /// Specifies that the file is not tracked.
    /// </summary>
    public static readonly ChangeIndicator Untracked = new(
        "Untracked",
        '?',
        colorOverrides: new Dictionary<ChangeArea, ChangeIndicatorColor>
        {
            [ChangeArea.Staging] = ChangeIndicatorColor.Working,
        });

    private ChangeIndicator(
        string name,
        char value,
        bool actionable = true,
        Dictionary<ChangeArea, ChangeIndicatorColor>? colorOverrides = default)
        : base(name, value)
    {
        Actionable = actionable;
        ColorOverrides = colorOverrides ?? [];
    }

    /// <summary>
    /// Gets a value indicating whether an action can be performed to resolve the changes.
    /// </summary>
    public bool Actionable { get; }

    /// <summary>
    /// Gets the color overrides by area.
    /// </summary>
    public IReadOnlyDictionary<ChangeArea, ChangeIndicatorColor> ColorOverrides { get; }
}
