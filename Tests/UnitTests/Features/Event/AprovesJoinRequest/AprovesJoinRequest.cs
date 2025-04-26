using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

public class AprovesJoinRequest {
    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the join request is approved
    //ID:UC22.S1
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_JoinRequestIsApproved() {
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


        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, @event.Participations.First().ParticipationStatus);
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the join request is not pending
    //ID:UC22.F1
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_JoinRequestIsNotPending() {
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
        @event.ApproveJoinRequest(guest);
        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.JoinRequestIsNotPending, result.Error);
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the event status is not active
    //ID:UC22.F2
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_EventStatusIsNotActive() {
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
        @event.CancelEvent();

        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the event time span is in the past
    //ID:UC22.F3
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_EventTimeSpanIsInPast() {
        //Arrange
        var @event = EventFactory.Init()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(5)
            .WithVisibility(EventVisibility.Private)
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();
        guest.RegisterToEvent(@event, "I want to join the event, and this is a valid reason");
        var newTimeSpan = EventDateTime.Create(
            DateTime.UtcNow.AddDays(-1).Date.AddHours(10), // Yesterday at 10 AM
            DateTime.UtcNow.AddDays(-1).Date.AddHours(11) // Yesterday at 11 AM
        );

        @event.TimeSpan = newTimeSpan.Payload;

        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventTimeSpanIsInPast, result.Error.GetAllErrors());
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the event is full
    //ID:UC22.F4
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_EventIsFull() {
        //Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(9)
            .WithVisibility(EventVisibility.Private)
            .Build();

        var guest1 = GuestFactory.Init("John", "Doe", "JDH@via.dk").Build();
        var guest2 = GuestFactory.Init("Jane", "Doe", "JNA@via.dk").Build();
        guest1.RegisterToEvent(@event, "I want to join the event, and this is a valid reason");
        guest2.RegisterToEvent(@event, "I want to join the event, and this is a valid reason");
        @event.ApproveJoinRequest(guest1);

        //Act
        var result = @event.ApproveJoinRequest(guest2);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the join request is not found
    //ID:UC22.F5
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_JoinRequestIsNotFound() {
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

        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.JoinRequestNotFound, result.Error);
    }

    // Given an event creator, a valid event and a valid user, when the event creator approves the join request, then the event is private and the user is not invited
    //ID:UC22.F6
    [Fact]
    public void EventCreatorApprovesJoinRequest_ValidEvent_ValidUser_EventIsPrivate_UserIsNotInvited() {
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

        //Act
        var result = @event.ApproveJoinRequest(guest);
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.JoinRequestNotFound, result.Error);
    }
}