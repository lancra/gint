using System.Text;
using Ardalis.SmartEnum;

namespace Gint.Dev.Targets;

internal sealed class VariablesTarget : ITarget
{
    private const string ForegroundResetEscapeCode = "\x1b[39m";
    private const string UnderlineStartEscapeCode = "\x1b[4m";
    private const string UnderlineEndEscapeCode = "\x1b[24m";

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            TargetKeys.Variables,
            "Shows variables used to modify target behaviors.",
            ExecuteAsync);

    private static async Task ExecuteAsync()
    {
        foreach (var variable in DevEnvironmentVariable.List)
        {
            var valueKind = VariableValueKind.FromVariable(variable);

            var builder = new StringBuilder()
                .Append(variable.Value)
                .Append(" = ")
                .Append(UnderlineStartEscapeCode)
                .Append(variable.ResultValue)
                .Append(UnderlineEndEscapeCode);

            if (!string.IsNullOrEmpty(variable.ResultValue))
            {
                builder.Append(' ');
            }

            builder.Append(valueKind.ForegroundEscapeCode)
                .Append($"({valueKind.Name})")
                .Append(ForegroundResetEscapeCode)
                .Append(": ")
                .Append(variable.Description);

            Console.WriteLine(builder.ToString());
        }
    }

    private sealed class VariableValueKind : SmartEnum<VariableValueKind>
    {
        public static readonly VariableValueKind Unset = new(1, "unset", 31);
        public static readonly VariableValueKind Default = new(2, "default", 33);
        public static readonly VariableValueKind System = new(3, "system", 32);

        private VariableValueKind(int value, string name, int foregroundEscapeCode)
            : base(name, value)
            => ForegroundEscapeCode = $"\x1b[{foregroundEscapeCode}m";

        public string ForegroundEscapeCode { get; }

        public static VariableValueKind FromVariable(DevEnvironmentVariable variable)
            => variable.SystemValue is not null
            ? System
            : variable.DefaultValue is not null
                ? Default
                : Unset;
    }
}
