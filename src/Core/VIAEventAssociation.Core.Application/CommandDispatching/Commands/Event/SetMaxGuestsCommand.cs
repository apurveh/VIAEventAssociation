using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;

public class SetMaxGuestsCommand : ICommand<EventId>
{
    public EventId Id { get; }
    public int MaxGuests { get; }

    private SetMaxGuestsCommand(EventId id, int maxGuests)
    {
        Id = id;
        MaxGuests = maxGuests;
    }

    public static Result<SetMaxGuestsCommand> Create(string eventIdAsString, int maxGuests)
    {
        var errors = new HashSet<Error>();
        var eventId = EventId.Create(eventIdAsString)
            .OnFailure(error => errors.Add(error));
        var guestCountResult = NumberOfGuests.Create(maxGuests);
        if (guestCountResult.IsFailure)
            errors.Add(guestCountResult.Error);
        if (errors.Any())
            return Error.Add(errors);

        return new SetMaxGuestsCommand(eventId.Payload, maxGuests);
    }
}