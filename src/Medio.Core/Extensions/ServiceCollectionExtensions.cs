using System.Reflection;
using Medio.Core.Abstractions;
using Medio.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Medio.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (assemblies == null || assemblies.Length == 0)
            throw new ArgumentException("You must provide at least one assembly to scan.", nameof(assemblies));

        services.AddSingleton<IMediator, Mediator>();

        var handlerInterfaceTypes = new[]
        {
            typeof(IRequestHandler<>),
            typeof(IRequestHandler<,>)
        };

        foreach (var assembly in assemblies)
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
                continue;

            var interfaces = type.GetInterfaces();

            foreach (var iface in interfaces)
            {
                if (!iface.IsGenericType)
                    continue;

                var genericDef = iface.GetGenericTypeDefinition();

                if (handlerInterfaceTypes.Contains(genericDef))
                    services.AddTransient(iface, type);
            }
        }

        return services;
    }
}