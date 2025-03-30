using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;

public class InviteGuestCommand : ICommand<EventId>
{
    public EventId Id { get; }
    public GuestId GuestId { get; }
    
    private InviteGuestCommand(EventId id, GuestId guestId)
    {
        Id = id;
        GuestId = guestId;
    }

    public static Result<InviteGuestCommand> Create(string eventIdAsString, string guestIdAsString)
    {
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