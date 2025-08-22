using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Services;

public class AddLocationToEvent {
    public static Result Handle(Event @event, Location location) {
        var errors = new HashSet<Error>();

        if (location.Events.Contains(@event))
            errors.Add(Error.EventAlreadyExistsInLocation);

        if (location.Events.Any(e => !(e.TimeSpan.End <= @event.TimeSpan.Start || e.TimeSpan.Start >= @event.TimeSpan.End)))
            errors.Add(Error.EventTimeSpanOverlapsWithAnotherEvent);

        if (IsEventWithinTimeSpan(@event, location.AvailableTimeSpan))
            errors.Add(Error.EventTimeSpanOutsideOfNewAvailability);

        location.AddEvent(@event)
            .OnSuccess(() => @event.AddLocation(location))
            .OnFailure(e => errors.Add(e));

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    private static bool IsEventWithinTimeSpan(Event @event, DateTimeRange availableTimeSpan) {
        return !(@event.TimeSpan.Start >= availableTimeSpan.Start && @event.TimeSpan.End <= availableTimeSpan.End);
    }
}