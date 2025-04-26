using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class RejectInvitationCommand : GuestCommand {
    private RejectInvitationCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }

    public static Result<RejectInvitationCommand> Create(string eventIdAsString, string guestIdAsString) {
        return Create(eventIdAsString, guestIdAsString,
            (eventId, guestId) => new RejectInvitationCommand(eventId, guestId));
    }
}