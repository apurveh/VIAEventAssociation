using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Dispatcher;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TId>(ICommand<TId> command);
}