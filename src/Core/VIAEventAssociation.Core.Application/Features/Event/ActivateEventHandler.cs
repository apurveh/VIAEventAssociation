using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class ActivateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<ActivateEventCommand>
{
    protected override Task<Result> PerformAction(global::Event eve, Command<EventId> command) {
        if (command is ActivateEventCommand activateEventCommand)
            return Task.FromResult(eve.Activate());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}