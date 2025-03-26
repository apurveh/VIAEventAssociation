using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching;

public interface ICommandHandler<TCommand, TId> where TCommand : ICommand<TId>
{
    Task<Result> HandleAsync(TCommand command);
}