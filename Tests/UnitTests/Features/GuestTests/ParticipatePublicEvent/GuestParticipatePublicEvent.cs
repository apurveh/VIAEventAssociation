using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.ParticipatePublicEvent;

public class GuestParticipatePublicEvent
{
    // UC11.S1
    [Fact]
    public void GuestParticipatePublicEvent_WithValidData_ShouldReturnSuccess()
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
        var result = guest.RegisterToEvent(@event);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, result.Payload.ParticipationStatus);
    }
    
    // UC11.F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Cancelled)]
    public void GuestParticipatePublicEvent_WithInvalidEventStatus_ShouldReturnFailure(EventStatus eventStatus)
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
        var result = guest.RegisterToEvent(@event);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }
    
    // UC11.F2
    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    public void GuestParticipatePublicEvent_WithMaxNumberOfGuests_ShouldReturnFailure(int maxNumberOfGuests)
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();
        
        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidConfirmedAttendees(maxNumberOfGuests)
            .Build();
        
        // Act
        var result = guest.RegisterToEvent(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }
    
    // UC11.F3
    [Fact]
    public void GuestParticipatePublicEvent_WithPastTimeEvent_ShouldReturnFailure()
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
        
        // Act
        var result = guest.RegisterToEvent(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsInPast, result.Error);
    }
    
    // UC11.F4
    [Fact]
    public void GuestParticipatePublicEvent_WithPrivateEventAndNoReason_ShouldReturnFailure()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();
        
        var @event = EventFactory
            .Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .Build();
        
        // Act
        var result = guest.RegisterToEvent(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsPrivate, result.Error);
    }
    
    // UC11.F5
    [Fact]
    public void GuestParticipatePublicEvent_WithAlreadyParticipatingGuest_ShouldReturnFailure()
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

        guest.RegisterToEvent(@event);
        
        // Act
        var result = guest.RegisterToEvent(@event);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.GuestAlreadyParticipating, result.Error);
    }
}