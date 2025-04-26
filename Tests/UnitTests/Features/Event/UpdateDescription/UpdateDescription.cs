using ViaEventAssociation.Core.Domain.Agregates.Events;

public class UpdateDescription {
    // Update description of an existing event, with a description length between 0 and 250 characters, and the event is in draft status
    // ID:UC3.S1
    [Theory]
    // 0 chars
    [InlineData("")]
    // 50 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a ar")]
    // 100 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio")]
    // 250 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor")]
    public void UpdateDescription_DescriptionLengthBetween0And250Characters_EventInDraftStatus_DescriptionUpdated(string description) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var convertedDescription = EventDescription.Create(description).Payload;

        // Act
        @event.UpdateDescription(convertedDescription);

        // Assert
        Assert.Equal(description, @event.Description.Value);
    }

    // Update description of an existing event, with a description length of 0 characters
    // ID:UC3.S2
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateDescription_DescriptionSetToNothing_DescriptionSetToEmpty(string description) {
        // Arrange
        var @event = EventFactory.Init().Build();
        var convertedDescription = EventDescription.Create(description).Payload;

        // Act
        @event.UpdateDescription(convertedDescription);

        // Assert
        Assert.Equal("", @event.Description.Value);
    }

    // Update description of an existing event, with a description length between 0 and 250 characters, and the event is in ready status
    // ID:UC3.S3
    [Theory]
    // 0 chars
    [InlineData("")]
    // 50 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a ar")]
    // 100 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio")]
    // 250 chars
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor")]
    public void UpdateDescription_DescriptionLengthBetween0And250Characters_EventInReadyStatus_DescriptionUpdated_EventInDraftStatus(string description) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Ready)
            .Build();
        var convertedDescription = EventDescription.Create(description).Payload;

        // Act
        @event.UpdateDescription(convertedDescription);

        // Assert
        Assert.Equal(description, @event.Description.Value);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    // Update description of an existing event, with a description length more than 250 characters
    // ID:UC3.F1
    [Fact]
    public void UpdateDescription_DescriptionLengthMoreThan250Characters_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var description = "Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor ";

        // Act
        var result = EventDescription.Create(description);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongDescription(250).Message, result.Error.Message);
    }

    // Update description of an existing event, and the event is in cancelled status
    // ID:UC3.F2
    [Fact]
    public void UpdateDescription_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();
        var updatedDescription = EventDescription.Create("New description").Payload;

        // Act
        var result = @event.UpdateDescription(updatedDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }

    // Update description of an existing event, and the event is in active status
    // ID:UC3.F3
    [Fact]
    public void UpdateDescription_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();
        var updatedDescription = EventDescription.Create("New description").Payload;

        // Act
        var result = @event.UpdateDescription(updatedDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }
}