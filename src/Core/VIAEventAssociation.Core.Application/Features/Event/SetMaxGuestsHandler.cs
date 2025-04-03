using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.Features.Event;

public class SetMaxGuestsHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork
) : EventHandler<SetMaxGuestsCommand>(eventRepository, unitOfWork)
{
    protected override Task<Result> PerformAction(Domain.Aggregates.Events.Event @event, SetMaxGuestsCommand command)
    {
        var result = @event.SetMaxNumberOfGuests(command.MaxGuests);
        return Task.FromResult(result);
    }
}