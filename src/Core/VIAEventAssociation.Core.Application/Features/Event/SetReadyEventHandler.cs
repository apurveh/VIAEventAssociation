using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class SetReadyEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<SetReadyEventCommand>
{
    protected override Task<Result> PerformAction(global::ViaEventAssociation.Core.Domain.Aggregates.Events.Event eve, Command<EventId> command) {
        if (command is SetReadyEventCommand setReadyEventCommand) return Task.FromResult(eve.SetReady());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}