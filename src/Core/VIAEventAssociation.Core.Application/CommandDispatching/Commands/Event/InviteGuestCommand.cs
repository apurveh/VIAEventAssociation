using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class InviteGuestCommand : Command<EventId> {
    private InviteGuestCommand(EventId id, GuestId guestId) : base(id) {
        GuestId = guestId;
    }

    public GuestId GuestId { get; }

    public static Result<InviteGuestCommand> Create(string eventIdAsString, string guestIdAsString) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(eventIdAsString)
            .OnFailure(error => errors.Add(error));

        var guestId = GuestId.Create(guestIdAsString)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new InviteGuestCommand(eventId.Payload, guestId.Payload);
    }
}