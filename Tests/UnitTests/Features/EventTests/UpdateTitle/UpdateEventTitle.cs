using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.UpdateEventTitleTests;

public class UpdateEventTitleTests
{
    // UC12.S1 - Successful title update
    [Fact]
    public void UpdateEventTitle_WithValidTitle_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var newTitle = "Updated Event Title";

        var result = @event.UpdateTitle(newTitle);
        
        Assert.True(result.IsSuccess == true);
        Assert.Equal(newTitle, @event.EventTitle.Value);
    }

    // UC12.F1 - Title cannot be empty
    [Fact]
    public void UpdateEventTitle_WithEmptyTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var invalidTitle = ""; 
        
        var result = @event.UpdateTitle(invalidTitle);
        
        Assert.True(result.IsFailure == true);
        Assert.Equal(Error.BlankString, result.Error);
    }

    // UC12.F2 - Title must be at least 3 characters
    [Fact]
    public void UpdateEventTitle_WithTooShortTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var invalidTitle = "AB"; 
        
        var result = @event.UpdateTitle(invalidTitle);
        
        Assert.True(result.IsFailure == true);
        Assert.Equal(Error.TooShortTitle(3), result.Error);
    }

    // UC12.F3 - Title must not exceed 50 characters
    [Fact]
    public void UpdateEventTitle_WithTooLongTitle_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .Build();

        var invalidTitle = new string('A', 51);
        
        var result = @event.UpdateTitle(invalidTitle);
        
        Assert.True(result.IsFailure == true);
        Assert.Equal(Error.TitleTooLong, result.Error);
    }

    // UC12.F4 - Title must not be null
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
}
