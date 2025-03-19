using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.EventTests.UpdateEventDescriptionTests;

public class UpdateEventDescriptionTests
{
    // UC3.S1 - Update description in Draft status with valid length (0-250 chars)
    [Fact]
    public void UpdateEventDescription_WithValidDescriptionInDraftStatus_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var newDescription = "Nullam tempor lacus nisl, eget tempus quam maximus malesuada. Morbi faucibus sed neque vitae euismod.";
        var result = @event.UpdateDescription(newDescription);

        Assert.True(result.IsSuccess);
        Assert.Equal(newDescription, @event.EventDescription.Value);
    }

    // UC3.S2 - Set description to empty string ("")
    [Fact]
    public void UpdateEventDescription_WithEmptyDescription_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var newDescription = "";
        var result = @event.UpdateDescription(newDescription);

        Assert.True(result.IsSuccess);
        Assert.Equal("", @event.EventDescription.Value);
    }

    // UC3.S3 - Update description in Ready status and ensure event becomes Draft
    [Fact]
    public void UpdateEventDescription_WithValidDescriptionInReadyStatus_ShouldChangeStatusToDraft()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Ready)
            .Build();

        var newDescription = "Vestibulum non purus vel justo ornare vulputate.";
        var result = @event.UpdateDescription(newDescription);

        Assert.True(result.IsSuccess, "Expected success when updating description in Ready status.");
        Assert.Equal(newDescription, @event.EventDescription.Value);
        Assert.Equal(EventStatus.Draft, @event.EventStatus); // ✅ Ensure status changes to Draft
    }

    
    // UC3.F1 - Description is too long (> 250 characters)
    [Fact]
    public void UpdateEventDescription_WithTooLongDescription_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var invalidDescription = new string('A', 251); // 251 characters
        var result = @event.UpdateDescription(invalidDescription);

        Assert.True(result.IsFailure, $"Expected failure but got success. Description: {invalidDescription}");
        Assert.Equal(Error.TooLongDescription(250).Message, result.Error.Message);
    }


    // UC3.F2 - Event is cancelled, should not allow description updates
    [Fact]
    public void UpdateEventDescription_WhenEventIsCancelled_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        var newDescription = "This event is still happening!";
        var result = @event.UpdateDescription(newDescription);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }

    // UC3.F3 - Event is active, should not allow description updates
    [Fact]
    public void UpdateEventDescription_WhenEventIsActive_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Active)
            .Build();

        var newDescription = "This is the updated event description.";
        var result = @event.UpdateDescription(newDescription);

        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsActive.Message, result.Error.Message);
    }
}