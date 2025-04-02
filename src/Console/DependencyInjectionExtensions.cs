using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Gint.Console.IO;
using Gint.Core.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gint.Console;

// See: https://anthonysimmon.com/true-dependency-injection-with-system-commandline
internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddConsole(this IServiceCollection services)
        => services.AddSingleton(ApplicationConsole.Console)
        .AddScoped<IRunPrompt, ConsoleRunPrompt>()
        .AddScoped<IPrinter, ConsolePrinter>();

    public static CommandLineBuilder UseDependencyInjection(
        this CommandLineBuilder builder,
        Action<IServiceCollection> configureServices)
        => UseDependencyInjection(builder, (_, services) => configureServices(services));

    public static CommandLineBuilder UseDependencyInjection(
        this CommandLineBuilder builder,
        Action<InvocationContext, IServiceCollection> configureServices)
        => builder.AddMiddleware(async (context, next)
            =>
        {
            var services = new ServiceCollection();
            configureServices(context, services);
            var uniqueServiceTypes = new HashSet<Type>(services.Select(service => service.ServiceType));

            services.TryAddSingleton(context.Console);

            var serviceProvider = services.BuildServiceProvider();
            await using (serviceProvider.ConfigureAwait(false))
            {
                context.BindingContext.AddService<IServiceProvider>(_ => serviceProvider);

                foreach (var serviceType in uniqueServiceTypes)
                {
                    context.BindingContext.AddService(serviceType, _ => serviceProvider.GetRequiredService(serviceType));

                    var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                    context.BindingContext.AddService(
                        enumerableServiceType,
                        _ => serviceProvider.GetRequiredService(enumerableServiceType));
                }

                await next(context)
                    .ConfigureAwait(false);
            }
        });
}
