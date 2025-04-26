using ViaEventAssociation.Core.Domain.Agregates.Events;

public class UpdateTitle {
    // Update title of an existing event, with a title length between 3 and 75 characters, and the event is in draft status
    // ID:UC2.S1
    [Theory]
    [InlineData("Scary Movie Night!")] // set the middle limit of the title length
    [InlineData("333")] // Set the min limit of the title length
    [InlineData("Creative minds inspire others, leading to innovation and positive change...")] // Set the max limit of the title lengthmax limit of the title length
    public void UpdateTitle_TitleLengthBetween3And75Characters_EventInDraftStatus_TitleUpdated(string title) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var convertedTitle = EventTitle.Create(title).Payload;

        // Act
        @event.UpdateTitle(convertedTitle);

        // Assert
        Assert.Equal(title, @event.Title.Value);
    }

    // Update title of an existing event, with a title length between 3 and 75 characters, and the event is in ready status
    // ID:UC2.S2
    [Fact]
    public void UpdateTitle_TitleLengthBetween3And75Characters_EventInReadyStatus_TitleUpdated_EventInDraftStatus() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Ready)
            .Build();

        var convertedTitle = EventTitle.Create("Graduation Gala").Payload;

        // Act
        @event.UpdateTitle(convertedTitle);

        // Assert
        Assert.Equal("Graduation Gala", @event.Title.Value);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    // Update title of an existing event, with a title length of 0 characters
    // ID:UC2.F1
    [Fact]
    public void UpdateTitle_TitleLength0Characters_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init().Build();

        // Act
        var result = EventTitle.Create("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message);
    }

    // Update title of an existing event, with a title length less than 3 characters
    // ID:UC2.F2
    [Theory]
    [InlineData("XY")]
    [InlineData("a")]
    public void UpdateTitle_TitleLengthLessThan3Characters_FailureMessageReturned(string title) {
        // Arrange
        var @event = EventFactory.Init().Build();

        // Act
        var result = EventTitle.Create(title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooShortTitle(3).Message, result.Error.Message);
    }

    // Update title of an existing event, with a title length more than 75 characters
    // ID:UC2.F3
    [Theory]
    [InlineData("Creative minds inspire others, leading to innovation and positive change....")]
    [InlineData("Creative minds inspire others, leading to innovation and positive change. Creative minds inspire others, leading to innovation and positive change.")] // 150 characters
    public void UpdateTitle_TitleLengthMoreThan75Characters_FailureMessageReturned(string title) {
        // Arrange
        var @event = EventFactory.Init().Build();

        // Act
        var result = EventTitle.Create(title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongTitle(75).Message, result.Error.Message);
    }

    // Update title of an existing event, with a title length more than 75 characters
    // ID:UC2.F4
    [Fact]
    public void UpdateTitle_TitleNull_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init().Build();
        string title = null;

        // Act
        var result = EventTitle.Create(title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullString.Message, result.Error.Message);
    }

    // Update title of an existing event, with a title length between 3 and 75 characters, and the event is in active status
    // ID:UC2.F5
    [Fact]
    public void UpdateTitle_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init().WithStatus(EventStatus.Active).Build();
        var convertedTitle = EventTitle.Create("Scary Movie Night!").Payload;

        // Act
        var result = @event.UpdateTitle(convertedTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }

    // Update title of an existing event, with a title length between 3 and 75 characters, and the event is in cancelled status
    // ID:UC2.F6
    [Fact]
    public void UpdateTitle_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();
        var convertedTitle = EventTitle.Create("Scary Movie Night!").Payload;

        // Act
        var result = @event.UpdateTitle(convertedTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}