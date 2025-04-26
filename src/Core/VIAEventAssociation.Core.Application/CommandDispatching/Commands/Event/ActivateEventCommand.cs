using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class ActivateEventCommand : Command<EventId> {
    public ActivateEventCommand(EventId eventId) : base(eventId) { }

    public static Result<ActivateEventCommand> Create(string id) {
        var eventId = EventId.Create(id);

        if (eventId.IsFailure)
            return eventId.Error;

        return new ActivateEventCommand(eventId.Payload);
    }
}