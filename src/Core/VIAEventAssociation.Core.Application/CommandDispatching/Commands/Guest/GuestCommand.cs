using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

public abstract class GuestCommand : Command<GuestId> {
    protected GuestCommand(EventId eventId, GuestId guestId) : base(guestId) {
        EventId = eventId;
    }

    public EventId EventId { get; private set; }

    public static Result<T> Create<T>(string eventIdAsString, string guestIdAsString, Func<EventId, GuestId, T> commandFactory) where T : GuestCommand {
        var errors = new HashSet<Error>();

        var eventIdResult = EventId.Create(eventIdAsString);
        var guestIdResult = GuestId.Create(guestIdAsString);

        if (eventIdResult.IsFailure) errors.Add(eventIdResult.Error);
        if (guestIdResult.IsFailure) errors.Add(guestIdResult.Error);

        if (errors.Any())
            return Error.Add(errors);

        return commandFactory(eventIdResult.Payload, guestIdResult.Payload);
    }
}