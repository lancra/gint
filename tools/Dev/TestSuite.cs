namespace Gint.Dev;

internal sealed record TestSuite(string Name, string Description, IReadOnlyCollection<TestProject> Projects)
{
    public override string ToString() => Name;
}
