using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider serviceProvider;
    
    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public async Task<Result> DispatchAsync<TId>(ICommand<TId> command)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TId));
        dynamic handler = serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        return await handler.HandleAsync((dynamic) command);
    }
}