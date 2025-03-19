using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.ActivateEvent;

public class ActivateEvent
{
    //UC9.S1
    [Fact]
    public void ActivateEvent_WhenEventIsDraftAndValid_ShouldMakeEventReadyThenActive()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.EventStatus);
    }

    // UC9.S2
    [Fact]
    public void ActivateEvent_WhenEventIsReadyAndValid_ShouldMakeEventActive()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Ready)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.EventStatus);
    }

    // UC9.S3
    [Fact]
    public void ActivateEvent_WhenEventIsAlreadyActive_ShouldRemainUnchanged()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.EventStatus);
    }

    // UC9.F1
    [Fact]
    public void ActivateEvent_whenEventIsDraft_AndTitleIsDefault_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTitleIsDefault, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }

    [Fact]
    public void ActivateEvent_WhenEventISDraft_AndDescriptionIsDefault_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventDescriptionIsDefault, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    // Times are not set or remains default value --This one is pending
    
    // Valid guest count is already checked in SetMaxGuests
    
    //UC9.F2
    [Fact]
    public void ReadyEvent_WhenEventIsCancelled_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.ActivateEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.CancelledEventCannotBeModified, result.Error); 
        Assert.Equal(EventStatus.Cancelled, @event.EventStatus);
    }
}