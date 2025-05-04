using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.QueryDispatching;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider serviceProvider;
    
    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public async Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        var queryType = query.GetType();
        var answerType = typeof(TAnswer);
        var handlerType = typeof(IQueryHandler<,>);
        
        var genericType = handlerType.MakeGenericType(queryType, answerType);
        
        dynamic handler = serviceProvider.GetService(genericType);

        return await handler.HandleAsync((dynamic)query);
    }

    public static void ListRegisteredServices(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider.GetServices<ServiceDescriptor>();
            foreach (var service in services)
            {
                Console.WriteLine($"{service.ServiceType.FullName} -> {service.ImplementationType?.FullName}");
            }
        }
    }
}