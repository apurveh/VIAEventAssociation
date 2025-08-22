using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class RequestToJoinCommand : GuestCommand {
    private RequestToJoinCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }

    public string? Reason { get; set; }

    public static Result<RequestToJoinCommand> Create(string eventIdAsString, string guestIdAsString, string? reason = null) {
        return Create(eventIdAsString, guestIdAsString,
            (eventId, guestId) => new RequestToJoinCommand(eventId, guestId) {
                Reason = reason
            });
    }
}