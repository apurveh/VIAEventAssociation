using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;


public class MakeEventPublicTests
{
    // S1 - Successfully make event public if it is in Draft, Ready, or Active status
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void MakeEventPublic_WhenStatusIsValid_ShouldReturnSuccess(EventStatus status)
    {
        var @event = EventFactory
            .Init()
            .WithStatus(status)
            .Build();

        var result = @event.MakePublic();

        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, @event.EventVisibility);
        Assert.Equal(status, @event.EventStatus); 
    }

    // F1 - Event is cancelled, making it public should fail
    [Fact]
    public void MakeEventPublic_WhenEventIsCancelled_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled) 
            .Build();

        var result = @event.MakePublic();

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.CancelledEventCannotBeModified, result.Error);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility); // Remains private
    }
}