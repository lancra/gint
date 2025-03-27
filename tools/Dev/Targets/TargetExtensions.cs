using Microsoft.Extensions.DependencyInjection;

namespace Gint.Dev.Targets;

internal static class TargetExtensions
{
    public static IServiceCollection AddTargets(this IServiceCollection services)
    {
        var serviceType = typeof(ITarget);
        var implementationTypes = serviceType.Assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(typeInterface => typeInterface == serviceType))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract)
            .ToArray();

        foreach (var implementationType in implementationTypes)
        {
            services.AddScoped(serviceType, implementationType);
        }

        return services;
    }
}
