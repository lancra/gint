namespace Gint.Dev;

internal sealed record TestProject(string Name, string Path)
{
    public override string ToString() => Name;
}
