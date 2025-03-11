using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.MakeEventPrivate;

public class MakeEventPrivate
{
    //UC6.S1
    [Fact]
    public void MakeEventPrivate_WhenEventIsAlreadyPrivateAndInDraft_ShouldRemainUnchanged()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Private)
            .WithStatus(EventStatus.Draft)
            .Build();
        
        var result = @event.MakePrivate();
        
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    [Fact]
    public void MakeEventPrivate_WhenEventIsAlreadyPrivateAndReady_ShouldRemainUnchanged()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Private)
            .WithStatus(EventStatus.Ready)
            .Build();
        
        var result = @event.MakePrivate();
        
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility);
        Assert.Equal(EventStatus.Ready, @event.EventStatus);
    }
    //UC6.S2
    [Fact]
    public void MakeEventPrivate_WhenEventIsPublicAndInDraft_ShouldChangeVisibilityToPrivate_AndStatusToDraft()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Draft)
            .Build();

        var @result = @event.MakePrivate();
        
        Assert.True(@result.IsSuccess);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    [Fact]
    public void MakeEventPrivate_WhenEventIsPublicAndReady_ShouldChangeVisibilityToPrivate_AndStatusToDraft()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Ready)
            .Build();

        var @result = @event.MakePrivate();
        
        Assert.True(@result.IsSuccess);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility);
        Assert.Equal(EventStatus.Draft, @event.EventStatus);
    }
    
    //UC6.F1
    [Fact]
    public void MakeEventPrivate_WhenStatusIsActive_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();
        
        var @result = @event.MakePrivate();
        
        Assert.True(@result.IsFailure);
        Assert.Equal(Error.ActiveEventCannotBeMadePrivate,@result.Error);
    }
    //UC6.F2
    [Fact]
    public void MakeEventPrivate_WhenStatusIsCancelled_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Cancelled)
            .Build();
        
        var @result = @event.MakePrivate();
        
        Assert.True(@result.IsFailure);
        Assert.Equal(Error.CancelledEventCannotBeModified,@result.Error);
    }
}