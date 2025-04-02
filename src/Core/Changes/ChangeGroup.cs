namespace Gint.Core.Changes;

/// <summary>
/// Represents all changes across areas.
/// </summary>
public record ChangeGroup
{
    private readonly Dictionary<ChangeArea, List<ChangeFile>> _actionableChangesByArea = ChangeArea.List
        .ToDictionary(area => area, area => new List<ChangeFile>());

    private readonly Dictionary<ChangeIndicator, bool> _changesByIndicator = ChangeIndicator.List
        .ToDictionary(indicator => indicator, indicator => false);

    internal ChangeGroup(IReadOnlyCollection<ChangeFile> files)
    {
        Files = files;

        foreach (var file in files)
        {
            foreach (var areaIndicator in file.Indicators)
            {
                _changesByIndicator[areaIndicator.Indicator] = true;
                if (areaIndicator.Indicator.Actionable)
                {
                    _actionableChangesByArea[areaIndicator.Area].Add(file);
                }
            }
        }
    }

    /// <summary>
    /// Gets the changes for each file.
    /// </summary>
    public IReadOnlyCollection<ChangeFile> Files { get; init; }

    /// <summary>
    /// Gets a value indicating whether actionable files are present.
    /// </summary>
    /// <param name="area">The area to check for actionable files.</param>
    /// <returns><c>true</c> when the area has actionable files; otherwise, <c>false</c>.</returns>
    public bool HasActionableFiles(ChangeArea area) => _actionableChangesByArea[area].Count > 0;

    /// <summary>
    /// Gets a value indicating whether an indicator is present.
    /// </summary>
    /// <param name="indicator">The indicator to check for.</param>
    /// <returns><c>true</c> when the indicator is present in the changes; otherwise, <c>false</c>.</returns>
    public bool HasIndicator(ChangeIndicator indicator) => _changesByIndicator[indicator];
}
