using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Agregates.Locations;

public class Location : AggregateRoot<LocationId> {
    private Location(LocationId locationId, LocationName name, NumberOfGuests maxNumberOfGuests) : base(locationId) {
        Name = name;
        MaxNumberOfGuests = maxNumberOfGuests;
        Events = new List<Event>();
    }

    private Location(LocationId locationId) : base(locationId) { }

    //Required by EF Core
    private Location() : base(default!) { } // Required by EF Core

    internal LocationName Name { get; set; }
    internal NumberOfGuests MaxNumberOfGuests { get; set; }
    internal List<Event> Events { get; }
    internal DateTimeRange AvailableTimeSpan { get; set; }

    public static Result<Location> Create() {
        try {
            var locationId = LocationId.GenerateId().Payload;
            var locationName = LocationName.Create("Working Title");
            if (locationName.IsFailure)
                return locationName.Error;
            var maxGuests = NumberOfGuests.Create(CONST.MIN_NUMBER_OF_GUESTS);
            if (maxGuests.IsFailure)
                return maxGuests.Error;
            return new Location(locationId, locationName.Payload, maxGuests.Payload);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateName(string name) {
        var locationName = LocationName.Create(name);
        if (locationName.IsFailure)
            return locationName.Error;
        Name = locationName.Payload;
        return Result.Ok;
    }

    public Result AddEvent(Event @event) {
        try {
            Events.Add(@event);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }

        return Result.Ok;
    }


    public Result SetMaxNumberOfGuests(int maxNumberOfGuests) {
        var maxGuests = NumberOfGuests.Create(maxNumberOfGuests);
        if (maxGuests.IsFailure)
            return maxGuests.Error;
        MaxNumberOfGuests = maxGuests.Payload;
        return Result.Ok;
    }


    public Result setsAvailableTimeSpan(DateTimeRange timeRange) {
        var errors = new HashSet<Error>();

        if (DateTimeRange.isPast(timeRange))
            errors.Add(Error.StartTimeIsInThePast);

        // Check if any event is outside the new availability time span.
        foreach (var @event in Events)
            if (!IsEventWithinTimeSpan(@event, timeRange)) {
                errors.Add(Error.EventTimeSpanOutsideOfNewAvailability);
                break; // Exit the loop once an out-of-range event is found for efficiency.
            }

        // Only update the available time span if there are no errors.
        if (errors.Any())
            return Result.Fail(Error.Add(errors));

        AvailableTimeSpan = timeRange;
        return Result.Ok;
    }

    private bool IsEventWithinTimeSpan(Event @event, DateTimeRange timeRange) {
        // Check if the event's time span is within the new availability time span.
        return @event.TimeSpan.Start >= timeRange.Start && @event.TimeSpan.End <= timeRange.End;
    }


    public bool isAvailable(DateTimeRange timeRange) {
        return Validate(timeRange).IsSuccess ? true : false;
    }

    private Result Validate(DateTimeRange timeRange) {
        var errors = new HashSet<Error>();

        if (AvailableTimeSpan.Start < timeRange.Start && AvailableTimeSpan.End > timeRange.End)
            errors.Add(Error.LocationNotAvailable);

        if (Events.Any(e => e.TimeSpan.Overlaps(timeRange)))
            errors.Add(Error.EventTimeSpanOverlapsWithAnotherEvent);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    public override string ToString() {
        return $"Location: {Name} - MaxNumberOfGuests: {MaxNumberOfGuests} - AvailableTimeSpan: {AvailableTimeSpan}";
    }
}