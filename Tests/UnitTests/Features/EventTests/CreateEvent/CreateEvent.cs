using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.EventTests.CreateNewEventTests;

public class CreateEvent
{
    // UC1.S1 - New event is created with ID, Draft status, and default max guests
    [Fact]
    public void CreateEvent_ShouldReturnEventWithIdAndDraftStatus()
    {
        var result = Event.Create();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.NotNull(result.Payload.Id);
        Assert.Equal(EventStatus.Draft, result.Payload.EventStatus);
        Assert.Equal(5, result.Payload.MaxNumberOfGuests.Value);  
    }

    // UC1.S2 - New event has default title "Working Title"
    [Fact]
    public void CreateEvent_ShouldHaveDefaultTitle_WorkingTitle()
    {
        var result = Event.Create();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal("Working Title", result.Payload.EventTitle.Value);  
    }

    // UC1.S3 - New event has an empty description
    [Fact]
    public void CreateEvent_ShouldHaveEmptyDescription()
    {
        var result = Event.Create();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal("", result.Payload.EventDescription.Value);  
    }

    // UC1.S4 - New event visibility is private
    [Fact]
    public void CreateEvent_ShouldHavePrivateVisibility()
    {
        var result = Event.Create();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(EventVisibility.Private, result.Payload.EventVisibility); 
    }
}