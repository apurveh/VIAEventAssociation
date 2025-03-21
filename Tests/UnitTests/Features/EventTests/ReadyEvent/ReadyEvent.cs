using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.ReadyEvent;

public class ReadyEvent
{
    //UC8.S1
    [Fact]
    public void ReadyEvent_WhenEventIsDraftAndValid_ShouldMakeEventReady()
    {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();
        
        //Act
        var result = @event.ReadyEvent();
        
        //Assrt
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, @event.EventStatus);
    }
    
    //UC8.F1 (Event is always assigned a title so it cannot be missing)
    [Fact]
    public void ReadyEvent_WhenEventISDraft_AndTitleIsDefault_ShouldReturnFailure() // This is also UC8.F4
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
        var result = @event.ReadyEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTitleIsDefault, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    [Fact]
    public void ReadyEvent_WhenEventISDraft_AndDescriptionIsDefault_ShouldReturnFailure()
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
        var result = @event.ReadyEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventDescriptionIsDefault, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    // UC8.F1
    [Fact]
    public void ReadyEvent_WhenEventIsDraft_AndTimeIsNotSet_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.ReadyEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidDateTimeRange, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }

    
    // Valid guest count is already checked in SetMaxGuests
  
    //UC8.F2
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
        var result = @event.ReadyEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.CancelledEventCannotBeModified, result.Error); 
        Assert.Equal(EventStatus.Cancelled, @event.EventStatus);
    }
    
    // UC8.F3
    [Fact]
    public void ReadyEvent_WhenEventIsInThePast_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInPast()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.ReadyEvent();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.PastEventsCannotBeModified, result.Error);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }

}