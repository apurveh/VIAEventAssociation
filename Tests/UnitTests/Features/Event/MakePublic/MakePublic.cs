using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePublic {
    // Given an existing event with ID, and the status is draft or ready or active, when creator chooses to make the event public, then the event is public and the status is unchanged
    // ID:UC5.S1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void MakePublic_EventInDraftStatus_EventIsPublic_StatusIsUnchanged(EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.MakePublic();

        // Assert
        Assert.Equal(EventVisibility.Public, @event.Visibility);
        Assert.Equal(status, @event.Status);
    }

    // Given an existing event with ID, and the event is in cancelled status, when creator chooses to make the event public, then a failure message is provided explaining a cancelled event cannot be modified
    // ID:UC5.F1
    [Fact]
    public void MakePublic_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.MakePublic();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}