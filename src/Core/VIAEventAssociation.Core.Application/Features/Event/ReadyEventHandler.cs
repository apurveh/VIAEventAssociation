using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Event;

public class ReadyEventHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork
) : EventHandler<ReadyEventCommand>(eventRepository, unitOfWork)
{
    protected override Task<Result> PerformAction(Domain.Aggregates.Events.Event @event, ReadyEventCommand command)
    {
        var result = @event.ReadyEvent();
        return Task.FromResult(result);
    }
}