using ViaEventAssociation.Core.Domain.Agregates.Events;

public class SetReadyEvent {
    // Given an event, when I set it to ready, then the event is ready
    // ID:UC8.S1
    [Fact]
    public void SetReadyEvent_EventIsDraft_EventIsReady() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithValidTitle()
            .Build();

        // Act
        @event.SetReady();

        // Assert
        Assert.Equal(EventStatus.Ready, @event.Status);
    }

    // Given an existing event with ID, and the event is in draft status, and the title is not set, when creator readies the event, then a failure message is provided explaining the title is missing
    // ID:UC8.F1
    [Fact]
    public void SetReadyEvent_EventIsDraft_TimesAreNotSet_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.SetReady();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTimeSpanIsNotSet.Message);
        Assert.Contains(Error.EventTimeSpanIsNotSet.Message, error.Message);
    }

    // Given an existing event with ID, and the event is in cancelled status, when creator readies the event, then a failure message is provided explaining a cancelled event cannot be readied
    // ID:UC8.F2
    [Fact]
    public void SetReadyEvent_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.SetReady();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventStatusIsCanceled.Message);
        Assert.Contains(Error.EventStatusIsCanceled.Message, error.Message);
    }

    // Given an existing event with ID, and the event has a start date/time which is prior to the time of readying, when the creator readies the event, then a failure message is provided explaining an event in the past cannot be made ready
    // ID:UC8.F3
    [Fact]
    public void SetReadyEvent_EventInPast_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInPast()
            .Build();

        // Act
        var result = @event.SetReady();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTimeSpanIsInPast.Message);
        Assert.Contains(Error.EventTimeSpanIsInPast.Message, error.Message);
    }


    // Given an existing event with ID, and the title of the event is the default (see UC1), when creator readies the event, then a failure message is provided explaining the title must changed from the default
    // ID:UC8.F4
    [Fact]
    public void SetReadyEvent_EventTitleIsDefault_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.SetReady();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTitleIsDefault.Message);
        Assert.Contains(Error.EventTitleIsDefault.Message, error.Message);
    }
}