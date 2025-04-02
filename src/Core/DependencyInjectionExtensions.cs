using Gint.Core.Changes;
using Gint.Core.External;
using Gint.Core.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace Gint.Core;

/// <summary>
/// Provides dependency injection support for the core library.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds the core library for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services.AddOperations()
        .AddScoped<ICommand, Command>()
        .AddScoped<IGitCommand, GitCommand>()
        .AddScoped<IChangeAccessor, ChangeAccessor>()
        .AddScoped<IOperationAccessor, OperationAccessor>()
        .AddScoped<IOperation, OperationHandler>();

    private static IServiceCollection AddOperations(this IServiceCollection services)
    {
        var serviceType = typeof(IInternalOperation);
        foreach (var descriptor in OperationDescriptor.List)
        {
            services.AddKeyedScoped(serviceType, descriptor.Value, descriptor.Type);
        }

        return services;
    }
}
