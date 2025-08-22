using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class CancelEventParticipationCommand : GuestCommand {
    private CancelEventParticipationCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }

    public static Result<CancelEventParticipationCommand> Create(string eventIdAsString, string guestIdAsString) {
        return Create(eventIdAsString, guestIdAsString,
            (eventId, guestId) => new CancelEventParticipationCommand(eventId, guestId));
    }
}