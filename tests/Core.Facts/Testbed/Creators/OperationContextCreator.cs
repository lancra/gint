using Gint.Core.Changes;
using Gint.Core.Operations;

namespace Gint.Core.Facts.Testbed.Creators;

internal static class OperationContextCreator
{
    public static OperationContext Create(
        Runspec? runspec = default,
        Pathspec? pathspec = default,
        OperationScope? scope = default,
        ChangeGroup? changes = default)
    {
        changes ??= new(
            [
                new(
                [
                    new(ChangeArea.Staging, ChangeIndicator.Unmodified),
                    new(ChangeArea.Working, ChangeIndicator.Modified),
                ],
                "foo.txt"),
            ]);

        return new(
            runspec ?? new(OperationDescriptor.Add, default),
            pathspec ?? new("foo.txt"),
            scope ?? OperationScope.All,
            changes);
    }
}
