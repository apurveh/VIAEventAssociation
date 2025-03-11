using UnitTests.Features.GuestTests;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.InviteGuest;

public class InviteGuest
{
    // UC13.S1
    [Theory]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void InviteGuest_WithValidEventAndGuest_ShouldRegisterPendingUser(EventStatus eventStatus)
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(eventStatus)
            .WithVisibility(EventVisibility.Public)
            .Build();
        
        // Act
        var result = @event.SendInvitation(guest);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Pending, result.Payload.ParticipationStatus);
    }
    
    // UC13.F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Cancelled)]
    public void InviteGuest_WithEventStatusDraftOrCancelled_ShouldReturnError(EventStatus eventStatus)
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(eventStatus)
            .WithVisibility(EventVisibility.Public)
            .Build();
        
        // Act
        var result = @event.SendInvitation(guest);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }
    
    // UC13.F2
    [Fact]
    public void InviteGuest_WithMaxNumberOfGuests_ShouldReturnError()
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
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(10)
            .Build();
        
        // Act
        var result = @event.SendInvitation(guest);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }
    
    // UC13.F3
    [Fact]
    public void InviteGuest_WithValidEventAndGuestAlreadyInvited_ShouldReturnError()
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
        
        @event.SendInvitation(guest);
        
        // Act
        var result = @event.SendInvitation(guest);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.GuestAlreadyInvited, result.Error);
    }
    
    // UC13.F4
    [Fact]
    public void InviteGuest_WithValidEventAndGuestAlreadyParticipating_ShouldReturnError()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidConfirmedAttendees(1)
            .Build();
        
        var guest = @event.Participations.First().Guest;
        
        // Act
        var result = @event.SendInvitation(guest);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.GuestAlreadyParticipating, result.Error);
    }
}