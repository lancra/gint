using Gint.Dev.Targets.Build;
using Microsoft.Extensions.DependencyInjection;

namespace Gint.Dev.Targets;

internal static class TargetExtensions
{
    public static IServiceCollection AddTargets(this IServiceCollection services)
        => services.AddTarget<DefaultTarget>()
        .AddBuildTargets()
        .AddTarget<LintTarget>();

    private static IServiceCollection AddBuildTargets(this IServiceCollection services)
        => services.AddTarget<BuildTarget>()
        .AddTarget<CleanTarget>()
        .AddTarget<CoverageTarget>()
        .AddTarget<DotnetTarget>()
        .AddTarget<PublishTarget>()
        .AddTarget<SolutionTarget>()
        .AddTarget<TestTarget>();

    private static IServiceCollection AddTarget<TTarget>(this IServiceCollection services)
        where TTarget : class, ITarget
        => services.AddScoped<ITarget, TTarget>();
}
