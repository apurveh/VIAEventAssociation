using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class AcceptsInvitationCommand : GuestCommand {
    private AcceptsInvitationCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }

    public static Result<AcceptsInvitationCommand> Create(string eventIdAsString, string guestIdAsString) {
        return Create(eventIdAsString, guestIdAsString,
            (eventId, guestId) => new AcceptsInvitationCommand(eventId, guestId));
    }
}