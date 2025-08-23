using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Extensions;

public static class QueryCollectionExtension
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services) {
        var handlerInterfaceType = typeof(IQueryHandler<,>);
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType))
            .ToList();

        foreach (var type in handlerTypes) {
            var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);
            services.AddScoped(interfaceType, type);

            Console.WriteLine($"Registered command handler: {type.Name} with service type: {interfaceType.Name}");
        }

        return services;
    }
}