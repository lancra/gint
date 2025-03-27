namespace Gint.Dev;

internal sealed record PublishProject(string Name, string Path)
{
    public override string ToString() => Name;
}
