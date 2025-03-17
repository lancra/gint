using Gint.Core.Changes;

namespace Gint.Testbed.Creators;

/// <summary>
/// Provides creation of changes representations for testing.
/// </summary>
public static class ChangesCreator
{
    private static int _counter;

    private static int Counter => _counter++;

    /// <summary>
    /// Creates a change group for testing.
    /// </summary>
    /// <param name="files">The change files in the group.</param>
    /// <returns>The created change group.</returns>
    public static ChangeGroup CreateGroup(params IReadOnlyCollection<ChangeFile> files)
        => new(files);

    /// <summary>
    /// Creates a change file for testing.
    /// </summary>
    /// <returns>The created change file.</returns>
    public static ChangeFile CreateFile() => CreateFile(areaIndicators: default);

    /// <summary>
    /// Creates a change file for testing.
    /// </summary>
    /// <param name="stagingIndicator">The change indicator for <see cref="ChangeArea.Staging"/>.</param>
    /// <param name="workingIndicator">The change indicator for <see cref="ChangeArea.Working"/>.</param>
    /// <param name="path">The path of the file.</param>
    /// <returns>The created change file.</returns>
    public static ChangeFile CreateFile(
        ChangeIndicator? stagingIndicator = default,
        ChangeIndicator? workingIndicator = default,
        string? path = default)
        => CreateFile(
            areaIndicators:
            [
                new(ChangeArea.Staging, stagingIndicator ?? ChangeIndicator.Unmodified),
                new(ChangeArea.Working, workingIndicator ?? ChangeIndicator.Modified),
            ],
            path: path);

    /// <summary>
    /// Creates a change file for testing.
    /// </summary>
    /// <param name="indicator">The chnage indicator for both areas.</param>
    /// <param name="path">The path of the file.</param>
    /// <returns>The created change file.</returns>
    public static ChangeFile CreateFile(
        ChangeIndicator indicator,
        string? path = default)
        => CreateFile(
            areaIndicators:
            [
                new(ChangeArea.Staging, indicator),
                new(ChangeArea.Working, indicator),
            ],
            path: path);

    /// <summary>
    /// Creates a change file for testing.
    /// </summary>
    /// <param name="areaIndicators">The change indicators for each area.</param>
    /// <param name="path">The path of the file.</param>
    /// <returns>The created change file.</returns>
    public static ChangeFile CreateFile(
        IReadOnlyCollection<ChangeAreaIndicator>? areaIndicators = default,
        string? path = default)
    {
        areaIndicators ??=
            [
                new(ChangeArea.Staging, ChangeIndicator.Unmodified),
                new(ChangeArea.Working, ChangeIndicator.Modified),
            ];

        return new(areaIndicators, path ?? $"file{Counter}.txt");
    }
}
