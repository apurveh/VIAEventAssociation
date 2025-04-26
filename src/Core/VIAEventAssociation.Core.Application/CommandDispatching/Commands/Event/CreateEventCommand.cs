using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class CreateEventCommand : Command<EventId> {
    private CreateEventCommand(EventId eventId) : base(eventId) { }

    public static Result<CreateEventCommand> Create() {
        var eventId = EventId.GenerateId();

        if (eventId.IsFailure)
            return eventId.Error;

        return new CreateEventCommand(eventId.Payload);
    }
}