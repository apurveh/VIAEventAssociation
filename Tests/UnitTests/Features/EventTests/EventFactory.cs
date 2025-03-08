using UnitTests.Features.GuestTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Values;

namespace UnitTests.Features.EventTests;

public class EventFactory
{
    private Event @event;
    
    private EventFactory() {}

    public static EventFactory Init()
    {
        var factory = new EventFactory();
        factory.@event = Event.Create().Payload;
        return factory;
    }
    
    public EventFactory WithStatus(EventStatus status)
    {
        @event.EventStatus = status;
        return this;
    }
    
    public EventFactory WithVisibility(EventVisibility visibility)
    {
        @event.EventVisibility = visibility;
        return this;
    }
    
    public Event Build()
    {
        return @event;
    }

    public EventFactory WithValidTimeInFuture()
    {
        var startDate = DateTime.Today.AddDays(1);
        var startTime = new TimeSpan(8, 20, 0);
        var startDateTime = startDate.Add(startTime);
        
        var endData = DateTime.Today.AddDays(1);
        var endTime = new TimeSpan(10, 20, 0);
        var endDateTime = endData.Add(endTime);

        var newTimeSpan = EventDateTime.Create(startDateTime, endDateTime)
            .OnFailure(error => throw new Exception("Invalid time span"));
        
        @event.EventTime = newTimeSpan.Payload;

        return this;
    }
    
    public EventFactory WithValidTimeInPast()
    {
        var startDate = DateTime.Today.AddDays(-1);
        var startTime = new TimeSpan(8, 20, 0);
        var startDateTime = startDate.Add(startTime);
        
        var endData = DateTime.Today.AddDays(-1);
        var endTime = new TimeSpan(10, 20, 0);
        var endDateTime = endData.Add(endTime);

        var newTimeSpan = EventDateTime.Create(startDateTime, endDateTime)
            .OnFailure(error => throw new Exception("Invalid time span"));
        
        @event.EventTime = newTimeSpan.Payload;

        return this;
    }

    public EventFactory WithValidTitle()
    {
        @event.EventTitle = EventTitle.Create("Valid Title").Payload;
        return this;
    }
    
    public EventFactory WithValidDescription()
    {
        @event.EventDescription = EventDescription.Create("Valid Description").Payload;
        return this;
    }
    
    public EventFactory WithMaxNumberOfGuests(int maxNumberOfGuests)
    {
        @event.MaxNumberOfGuests = NumberOfGuests.Create(maxNumberOfGuests).Payload;
        return this;
    }

    public EventFactory WithValidConfirmedAttendees(int confirmedAttendees)
    {
        var participants = 999999;
        for (var i = participants; i > participants - confirmedAttendees; i--)
            CreateAndRegisterGuest("John", "Doe", $"{i}@via.dk");
        return this;
    }

    private void CreateAndRegisterGuest(string firstName, string lastName, string email)
    {
        var guest = GuestFactory.Init(firstName, lastName, email).Build();
        guest.RegisterToEvent(@event);
    }
}