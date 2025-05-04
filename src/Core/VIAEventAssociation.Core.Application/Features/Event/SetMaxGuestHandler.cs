using ViaEventAssociation.Core.Application.CommandDispatching;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class SetMaxGuestHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork), ICommandHandler<SetMaxGuestCommand>
{
    protected override Task<Result> PerformAction(global::Event @event, Command<EventId> command) {
        if (command is SetMaxGuestCommand setMaxGuestCommand)
            return Task.FromResult(@event.SetMaxGuests(setMaxGuestCommand.MaxGuests));
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}