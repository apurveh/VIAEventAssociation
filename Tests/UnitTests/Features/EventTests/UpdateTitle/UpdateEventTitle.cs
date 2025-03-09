using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.EventTests.UpdateEventTitleTests;

public class UpdateEventTitleTests
{
    // UC2.S1 - Successfully update title (Draft status)
    [Fact]
    public void UpdateEventTitle_WithValidTitleInDraftStatus_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Draft)
            .Build();

        var newTitle = "Scary Movie Night";
        var result = @event.UpdateTitle(newTitle);

        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, @event.EventTitle.Value);
    }

    // UC2.S2 - Successfully update title (Ready status)
    [Fact]
    public void UpdateEventTitle_WithValidTitleInReadyStatus_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Ready)
            .Build();

        var newTitle = "Graduation Gala";
        var result = @event.UpdateTitle(newTitle);

        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, @event.EventTitle.Value);
        Assert.Equal(EventStatus.Ready, @event.EventStatus);
    }

    // UC2.F1 - Title is empty (0 characters)
    [Fact]
    public void UpdateEventTitle_WithEmptyTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var invalidTitle = "";
        var result = @event.UpdateTitle(invalidTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.TooShortTitle(3).Message, result.Error.Message);
    }

    // UC2.F2 - Title is too short (< 3 characters)
    [Theory]
    [InlineData("XY")]
    [InlineData("a")]
    public void UpdateEventTitle_WithTooShortTitle_ShouldReturnFailure(string invalidTitle)
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var result = @event.UpdateTitle(invalidTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.TooShortTitle(3).Message, result.Error.Message);
    }

    // UC2.F3 - Title is too long (> 75 characters)
    [Fact]
    public void UpdateEventTitle_WithTooLongTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var invalidTitle = new string('A', 76);
        var result = @event.UpdateTitle(invalidTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.TitleTooLong.Message, result.Error.Message);
    }

    // UC2.F4 - Title is null
    [Fact]
    public void UpdateEventTitle_WithNullTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        string? invalidTitle = null;
        var result = @event.UpdateTitle(invalidTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.NullString.Message, result.Error.Message);
    }

    // UC2.F5 - Event is active (Cannot modify)
    [Fact]
    public void UpdateEventTitle_WhenEventIsActive_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .Build();

        var newTitle = "New Event Title";
        var result = @event.UpdateTitle(newTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsActive.Message, result.Error.Message);
    }

    // UC2.F6 - Event is cancelled (Cannot modify)
    [Fact]
    public void UpdateEventTitle_WhenEventIsCancelled_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        var newTitle = "New Event Title";
        var result = @event.UpdateTitle(newTitle);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}
