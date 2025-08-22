using ViaEventAssociation.Core.Domain.Aggregates.Events;

public class CreateEvent {
    // Create event with null id should produce draft status, and the maximum number of guests is 5
    // ID:UC1.S1
    [Fact]
    public void CreateEvent_WithNullId_ProduceDraftStatus_AndTheMaximumNumberOfGuestsIs5() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal(EventStatus.Draft, @event.Status);
        Assert.Equal(5, @event.MaxNumberOfGuests.Value);
    }

    // Create event with null id should produce title "Working Title"
    // ID:UC1.S2
    [Fact]
    public void CreateEvent_WithNullId_ProduceTitleWorkingTitle() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal("Working Title", @event.Title.Value);
    }

    // Create event with null id should produce private visibility
    // ID:UC1.S3
    [Fact]
    public void CreateEvent_WithNullId_ProduceEmptyDescription() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert

        Assert.Equal(string.Empty, @event.Description.Value);
    }

    //S4

    [Fact]
    public void CreateEvent_WithNullId_ProducePrivateVisibility() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal(EventVisibility.Private, @event.Visibility);
    }
}