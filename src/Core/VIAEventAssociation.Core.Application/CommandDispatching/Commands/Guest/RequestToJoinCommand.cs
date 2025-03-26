using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class RequestToJoinCommand : GuestCommand
{
    public string? Reason { get; set; }
    
    private RequestToJoinCommand(EventId eventId, GuestId guestId) : base(eventId, guestId) { }
    
    public static Result<RequestToJoinCommand> Create(string eventIdAsString, string guestIdAsString, string? reason = null) {
        return Create(eventIdAsString, guestIdAsString, (eventId, guestId) => 
            new RequestToJoinCommand(eventId, guestId) {
                Reason = reason 
            });
    }
}