using ViaEventAssociation.Core.Application.CommandDispatching.Commands;

namespace ViaEventAssociation.Core.Application.Features.Dispatcher;

public interface ICommandDispatcher {
    Task<Result> DispatchAsync(Command command);
}