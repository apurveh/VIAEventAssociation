using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class ActivateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<ActivateEventCommand>
{
    protected override Task<Result> PerformAction(Domain.Aggregates.Events.Event eve, Command<EventId> command) {
        if (command is ActivateEventCommand activateEventCommand)
            return Task.FromResult(eve.Activate());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}