using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public abstract class GuestCommand(EventId eventId, GuestId guestId) : ICommand<GuestId>
{
    public GuestId Id { get; } = guestId;
    public EventId EventId { get; } = eventId;

    public static Result<T> Create<T>(string eventIdAsString, string guestIdAsString,
        Func<EventId, GuestId, T> commandFactory) where T : GuestCommand
    {
        var errors = new HashSet<Error>();
        
        var eventIdResult = EventId.Create(eventIdAsString);
        var guestIdResult = GuestId.Create(guestIdAsString);
        
        if (eventIdResult.IsFailure)
            errors.Add(eventIdResult.Error);
        
        if (guestIdResult.IsFailure)
            errors.Add(guestIdResult.Error);

        if (errors.Any())
            return Error.Add(errors);
        
        return commandFactory(eventIdResult.Payload, guestIdResult.Payload);
    }
}