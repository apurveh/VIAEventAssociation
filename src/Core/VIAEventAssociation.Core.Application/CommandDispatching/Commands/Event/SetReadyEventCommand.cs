using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

public class SetReadyEventCommand : Command<EventId> {
    public SetReadyEventCommand(EventId eventId) : base(eventId) { }

    public static Result<SetReadyEventCommand> Create(string id) {
        var eventId = EventId.Create(id);

        if (eventId.IsFailure)
            return eventId.Error;

        return new SetReadyEventCommand(eventId.Payload);
    }
}