using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Domain.Common.Values;

public class LocationFactory {
    private Location _location;

    private LocationFactory() { }

    public static LocationFactory Init() {
        var factory = new LocationFactory();
        factory._location = Location.Create().Payload;
        return factory;
    }

    public LocationFactory WithName(string name) {
        _location.Name = LocationName.Create(name).Payload;
        return this;
    }

    public LocationFactory WithMaxNumberOfGuests(int maxNumberOfGuests) {
        _location.MaxNumberOfGuests = NumberOfGuests.Create(maxNumberOfGuests).Payload;
        return this;
    }

    public LocationFactory WithEvent(Event @event) {
        _location.Events.Add(@event);
        return this;
    }

    public Location Build() {
        return _location;
    }

    public LocationFactory WithAvailableTimeSpan(DateTimeRange dateTimeRange) {
        _location.AvailableTimeSpan = dateTimeRange;
        return this;
    }

    public LocationFactory WithAvailableTimeSpan(DateTime start, DateTime end) {
        _location.AvailableTimeSpan = DateTimeRange.Create(start, end).Payload;
        return this;
    }

    public LocationFactory WithEventConfirmed(DateTime start, DateTime end, int maxNumberOfGuests) {
        var @event = EventFactory.Init()
            .WithTimeSpan(start, end)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .Build();
        _location.Events.Add(@event);
        return this;
    }
}