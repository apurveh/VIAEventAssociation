using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivate {
    //Given an existing event with ID, and the status is draft or ready, when creator chooses to make the event private, then the event is private and the status is unchanged
    //ID:UC6.S1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void MakePrivate_EventInDraftOrReadyStatus_EventIsPrivate_StatusIsUnchanged(EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .WithVisibility(EventVisibility.Private)
            .Build();

        // Act
        @event.MakePrivate();

        // Assert
        Assert.Equal(EventVisibility.Private, @event.Visibility);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    //Given an existing event with ID, and the status is draft or ready, when creator chooses to make the event private, then the event is private and the status is unchanged
    //ID:UC6.S2
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void MakePrivate_EventInDraftOrReadyStatus_EventIsPublic_EventIsPrivate_StatusIsUnchanged(EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .WithVisibility(EventVisibility.Public)
            .Build();

        // Act
        @event.MakePrivate();

        // Assert
        Assert.Equal(EventVisibility.Private, @event.Visibility);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    //Given an existing event with ID, and the event is in active status, when creator chooses to make the event private, then a failure message is provided explaining an active event cannot be made private
    //ID:UC6.F1
    [Fact]
    public void MakePrivate_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        var result = @event.MakePrivate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }

    //Given an existing event with ID, and the event is in cancelled status, when creator chooses to make the event private, then a failure message is provided explaining a cancelled event cannot be modified
    //ID:UC6.F2
    [Fact]
    public void MakePrivate_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.MakePrivate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}