namespace Gint.Dev;

internal sealed class DevEnvironmentVariable
{
    private const string Prefix = "GINT_";

    private static readonly string[] TrueValues =
    [
        "1",
        "on",
        "true",
        "yes",
    ];

    private bool _hydratedValue;
    private string? _value;

    public DevEnvironmentVariable(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Name = Prefix + name;
    }

    public string Name { get; }

    public string? Value
    {
        get
        {
            if (!_hydratedValue)
            {
                _value = Environment.GetEnvironmentVariable(Name);
                _hydratedValue = true;
            }

            return _value;
        }
    }

    public bool IsTruthy
        => Value is not null &&
        TrueValues.Contains(Value, StringComparer.OrdinalIgnoreCase);
}
