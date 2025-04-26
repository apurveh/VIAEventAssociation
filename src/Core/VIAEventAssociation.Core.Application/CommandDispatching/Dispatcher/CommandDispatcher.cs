using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Dispatcher;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher;

public class CommandDispatcher : ICommandDispatcher {
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> DispatchAsync(Command command) {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        return await handler.HandleAsync((dynamic) command);
    }
}