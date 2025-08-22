using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace UnitTests.Features.Event.DeclinesJoinRequest;

public class DeclinesJoinRequest {
    // Given an event creator, a valid event and a valid user, when the event creator declines the join request, then the join request is declined
    //ID:UC23.S1
    [Fact]
    public void EventCreatorDeclinesJoinRequest_ValidEvent_ValidUser_JoinRequestIsDeclined() {
        //Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(10)
            .WithVisibility(EventVisibility.Private)
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();
        guest.RegisterToEvent(@event, "I want to join the event, and this is a valid reason");

        //Act
        var result = @event.DeclineJoinRequest(guest);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Declined, @event.Participations.First().ParticipationStatus);
    }
    

    // Given an event creator, a valid event and a valid user, when the event creator declines the join request, then the event time span is in the past
    //ID:UC23.F2
    [Fact]
    public void EventCreatorDeclinesJoinRequest_ValidEvent_ValidUser_EventTimeSpanIsInPast() {
        //Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(5)
            .WithVisibility(EventVisibility.Private)
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();

        guest.RegisterToEvent(@event, "I want to join the event, and this is a valid reason");

        var start = new DateTime(2025, 08, 20, 12, 0, 0, DateTimeKind.Utc);
        var end   = new DateTime(2025, 08, 20, 14, 0, 0, DateTimeKind.Utc);

        @event.TimeSpan = EventDateTime.Create(
            start,end
        ).Payload;

        //Act
        var result = @event.DeclineJoinRequest(guest);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsInPast, result.Error);
    }

    // Given an event creator, a valid event and a valid user, when the event creator declines the join request, then the join request is not found
    //ID:UC23.F4
    [Fact]
    public void EventCreatorDeclinesJoinRequest_ValidEvent_ValidUser_JoinRequestIsNotFound() {
        //Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(10)
            .WithVisibility(EventVisibility.Private)
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();

        //Act
        var result = @event.DeclineJoinRequest(guest);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.JoinRequestNotFound, result.Error);
    }
}