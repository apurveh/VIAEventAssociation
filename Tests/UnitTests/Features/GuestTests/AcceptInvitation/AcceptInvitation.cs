using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.AcceptInvitation;

public class AcceptInvitation
{
    // UC14.S1
    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    public void AcceptInvitation_WithValidEventAndGuestAndPendingInvitation_ShouldChangeInvitationStatus(int maxNumberOfGuests)
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithValidConfirmedAttendees(maxNumberOfGuests - 1)
            .Build();

        @event.SendInvitation(guest);
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, guest.Participations.First().ParticipationStatus);
    }
    
    // UC14.F1
    [Fact]
    public void AcceptInvitation_WithValidEventAndGuestNotInvited_ShouldReturnError()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvitationPendingNotFound, result.Error);
    }
    
    // UC14.F2
    [Fact]
    public void AcceptInvitation_WithValidEventAndGuestAndTooManyGuests_ShouldReturnError()
    {
        // Arrange
        var guestOne = GuestFactory
            .Init("John", "Doe", "111111@via.dk")
            .Build();
        
        var guestTwo = GuestFactory
            .Init("John", "Doe", "222222@via.dk")
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(9)
            .Build();

        @event.SendInvitation(guestOne);
        @event.SendInvitation(guestTwo);
        guestOne.AcceptInvitation(@event);
        
        // Act
        var result = guestTwo.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }
    
    // UC14.F3
    [Fact]
    public void AcceptInvitation_WithValidEventAndGuestAndEventIsCancelled_ShouldReturnError()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Cancelled)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }
    
    // UC14.F4
    [Fact]
    public void AcceptInvitation_WithValidEventAndGuestAndEventIsReady_ShouldReturnError()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Ready)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }
    
    // UC14.F5
    [Fact]
    public void AcceptInvitation_WithValidEventAndDataAndTimeIsPast_ShouldReturnError()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInPast()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);
        
        // Act
        var result = guest.AcceptInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsInPast, result.Error);
    }
}