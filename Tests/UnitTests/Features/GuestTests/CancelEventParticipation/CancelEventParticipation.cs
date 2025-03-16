using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.CancelEventParticipation;

public class CancelEventParticipation
{
    // UC12.S1
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuest_ShouldRemoveParticipation()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();

        guest.RegisterToEvent(@event);
        
        // Act
        var result = guest.CancelParticipation(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    // UC12.S2
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuestNotParticipating_ShouldRemoveParticipation()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        var result = guest.CancelParticipation(@event);

        // Assert
        Assert.Equal(Error.GuestNotFound, result.Error);
    }
    
    // UC12.F1
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuestAndEventIsPast_ShouldReturnError()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .WithValidConfirmedAttendees(1)
            .WithValidTimeInPast()
            .Build();

        var guest = @event.Participations.First().Guest;
        
        // Act
        var result = guest.CancelParticipation(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsPast, result.Error);
    }
}