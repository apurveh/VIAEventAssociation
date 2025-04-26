using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

public class SetMaxGuestCommand : Command<EventId> {
    public SetMaxGuestCommand(EventId id, int maxGuests) : base(id) {
        MaxGuests = maxGuests;
    }

    public int MaxGuests { get; }

    public static Result<SetMaxGuestCommand> Create(string id, string maxGuests) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(id)
            .OnFailure(error => errors.Add(error));

        var maxGuestsInt = int.TryParse(maxGuests, out var maxGuestsIntResult) ? maxGuestsIntResult : -1;

        if (maxGuestsInt < 0)
            errors.Add(Error.InvalidMaxGuests(maxGuests));

        if (errors.Any())
            return Error.Add(errors);

        return new SetMaxGuestCommand(eventId.Payload, maxGuestsInt);
    }
}