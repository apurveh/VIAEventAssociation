using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class AcceptInvitationCommand : GuestCommand
{
    private AcceptInvitationCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }

    public static Result<AcceptInvitationCommand> Create(string eventIdAsString, string guestIdAsString)
    {
        return Create(eventIdAsString, guestIdAsString, (eventId, guestId) =>
            new AcceptInvitationCommand(eventId, guestId));
    }
}