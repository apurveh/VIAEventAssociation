using System.Linq;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Features.Event.ActivateEvent;

public class ActivateEvent {
    private readonly ITestOutputHelper _testOutputHelper;

    public ActivateEvent(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    // Given an existing event with ID, and the event is in draft status, and the title is set, when creator activates the event, then the event is active
    // ID:UC9.F1
    [Fact]
    public void ActivateEvent_EventIsDraft_TitleIsNotSet_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(5)
            .Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // Given an existing event with ID, and the event is in ready status, when creator activates the event, then the event is active
    // ID:UC9.S2
    [Fact]
    public void ActivateEvent_EventIsReady_EventIsActive() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Ready)
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(5)
            .Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // Given an existing event with ID, and the event is in active status, when creator activates the event, then the event is active
    // ID:UC9.S3
    [Fact]
    public void ActivateEvent_EventIsActive_EventIsActive() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(5)
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        _testOutputHelper.WriteLine(@event.TimeSpan.ToString());
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        // _testOutputHelper.WriteLine(@event.TimeSpan.ToString());
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // Given an existing event with ID, and the event is in draft status, and the title is not set, when creator activates the event, then a failure message is provided explaining the title is missing
    // ID:UC9.F1
    [Fact]
    public void ActivateEvent_EventIsDraft_TimesAreNotSet_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTimeSpanIsNotSet.Message);
        Assert.Contains(Error.EventTimeSpanIsNotSet.Message, error.Message);
    }

    // Given an existing event with ID, and the event is in cancelled status, when creator activates the event, then a failure message is provided explaining a cancelled event cannot be activated
    // ID:UC9.F2
    [Fact]
    public void ActivateEvent_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventStatusIsCanceled.Message);
        Assert.Contains(Error.EventStatusIsCanceled.Message, error.Message);
    }
}