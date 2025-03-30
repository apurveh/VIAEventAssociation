using VIAEventAssociation.Core.Application.CommandDispatching;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Event;

public abstract class EventHandler<TCommand>(IEventRepository eventRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<TCommand, EventId> 
    where TCommand : ICommand<EventId>
{
    protected readonly IEventRepository Repository = eventRepository;
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(TCommand command)
    {
        var result = await Repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var @event = result.Payload;

        if (@event is null)
            return Error.EventIsNotFound;
        
        var action = await PerformAction(@event, command);
        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
    
    protected abstract Task<Result> PerformAction(Domain.Aggregates.Events.Event @event, TCommand command);
}