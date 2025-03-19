using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.DeclineInvitation;

public class DeclineInvitation
{
    // UC15.S1
    [Fact]
    public void DeclineInvitation_WithValidEventAndGuestAndPendingInvitation_ShouldChangeInvitationStatus()
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
        var result = guest.DeclineInvitation(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
        // TODO: Ask Troels maybe it is better for DeclineInvitation and AcceptInvitation to return back Result<Participation> instead of Result so that we have direct access to result.Payload.ParticipationStatus
        Assert.Equal(ParticipationStatus.Declined, guest.Participations.First().ParticipationStatus);
    }
    
    // UC15.S2
    [Fact]
    public void DeclineInvitation_WithValidEventAndGuestAndAcceptedInvitation_ShouldChangeInvitationStatus()
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
        guest.AcceptInvitation(@event);
        
        // Act
        var result = guest.DeclineInvitation(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Declined, guest.Participations.First().ParticipationStatus);
    }
    
    // UC15.F1
    [Fact]
    public void DeclineInvitation_WithValidEventAndGuestNotInvited_ShouldReturnError()
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
        var result = guest.DeclineInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvitationPendingOrAcceptedNotFound, result.Error);
    }
    
    // UC15.F2
    [Fact]
    public void DeclineInvitation_WithCancelledEventAndGuest_ShouldReturnError()
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
        var result = guest.DeclineInvitation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceledAndCannotRejectInvitation, result.Error);
    }
}