using Ardalis.SmartEnum;

namespace Gint.Dev;

internal sealed class DevEnvironmentVariable : SmartEnum<DevEnvironmentVariable, string>
{
    public static readonly DevEnvironmentVariable BuildConfiguration =
        new("BUILD_CONFIGURATION", "The .NET build configuration.", "Release");

    public static readonly DevEnvironmentVariable LocalBuild =
        new("LOCAL_BUILD", "Denotes that the build should be more permissive to warnings.");

    public static readonly DevEnvironmentVariable LocalLint =
        new("LOCAL_LINT", "Denotes that linters should fix issues where applicable.");

    public static readonly DevEnvironmentVariable VirtualizationPlatform =
        new("CONTAINER_RUNTIME", "The container runtime to use for target commands.", "podman");

    private const string Prefix = "GINT_";

    private static readonly string[] TrueValues =
    [
        "1",
        "on",
        "true",
        "yes",
    ];

    public DevEnvironmentVariable(string name, string description)
        : base(name, Prefix + name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(description);
        Description = description;
    }

    public DevEnvironmentVariable(string name, string description, string defaultValue)
        : this(name, description)
    {
        ArgumentException.ThrowIfNullOrEmpty(defaultValue);
        DefaultValue = defaultValue;
    }

    public string Description { get; }

    public string? DefaultValue { get; }

    public string? SystemValue => Environment.GetEnvironmentVariable(Value);

    public string ResultValue
        => SystemValue
        ?? DefaultValue
        ?? string.Empty;

    public bool IsTruthy
        => TrueValues.Contains(ResultValue, StringComparer.OrdinalIgnoreCase);
}
