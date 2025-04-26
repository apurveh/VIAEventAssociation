using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace UnitTests.Features.Guest.GuestRequestToJoin;

public class GuestRequestToJoinPrivate {
    // Given an existing event with ID, and the event status is active, and the event is private, and a registered guest with ID and a valid reason , and the current number of registered guests is less than the maximum number of allowed guests, and the event has not yet started, i.e. before the start time, when the guest requests to join the private event, then the event has registered that the guest intends to participate
    // ID:UC21.S1
    [Theory]
    [InlineData("I want to join, and this reason has more than 25 characters")]
    [InlineData("I want to join, and this reason has more than 25 characters, and this is a very long reason")]
    public void GuestRequestToJoinPrivate_WithValidData_WithaValidReadon_ShouldReturnSuccess(string reason) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .WithMaxNumberOfGuests(10)
            .WithValidTimeInFuture()
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event, reason);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Pending, result.Payload.ParticipationStatus);
    }

    //Given an existing valid event with ID, and the event status is draft, ready, or cancelled, and a registered guest with ID, and the event is private, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that only active events can be joined
    //ID:UC21.F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Cancelled)]
    public void GuestRequestToJoinPrivate_WithInvalidEventStatus_ShouldReturnFailure(EventStatus eventStatus) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(eventStatus)
            .WithVisibility(EventVisibility.Private)
            .WithValidTimeInFuture()
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }

    //Given an existing valid event with ID, and the event status is active, and a registered guest with ID, and the current number of registered guests (participants, invitees who have accepted, and accepted participate-requests) is equal to the maximum number of allowed guests, and the event is private, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that there is no more room
    //ID:UC21.F2
    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    public void GuestRequestToJoinPrivate_WithMaxNumberOfGuests_WithAValidReadon_ShouldReturnFailure(int maxNumberOfGuests) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var newEvent = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithValidTimeInFuture()
            .WithValidConfirmedAttendees(maxNumberOfGuests)
            .WithVisibility(EventVisibility.Private)
            .Build();

        //Act
        var result = guest.RegisterToEvent(newEvent, "I want to join, and this reason has more than 25 characters");

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }

    //Given an existing valid event with ID, and the event start time is in the past, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that only future events can be participated
    //ID:UC21.F3
    [Fact]
    public void GuestRequestToJoinPrivate_WithPastEvent_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .WithValidTimeInPast()
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsInPast, result.Error);
    }

    //Given an existing valid event with ID, and a registered guest with ID, and the event is private. and the user has a non valid reason, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that the reason is not valid
    //ID:UC21.F4
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("please")]
    public void GuestRequestToJoinPrivate_WithInvalidReason_ShouldReturnFailure(string reason) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .WithValidTimeInFuture()
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event, reason);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsPrivate, result.Error);
    }

    //Given an existing valid event with ID, and a registered guest with ID, and the guest is already a participant at the event, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that a guest cannot take two slots at an event
    //ID:UC21.F5
    [Fact]
    public void GuestRequestToJoinPrivate_WithAlreadyParticipatingGuest_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .WithValidTimeInFuture()
            .Build();

        guest.RegisterToEvent(@event, "I want to join, and this reason has more than 25 characters");

        //Act
        var result = guest.RegisterToEvent(@event, "I want to join, and this reason has more than 25 characters");

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.GuestAlreadyParticipating, result.Error);
    }
}