namespace UnitTests.Features.EventTests.MakeEventPrivate;
using Fakes;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Application.Features.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

public class MakeEventPrivateHandlerTest
{
    // UC6.S1
    [Fact]
    public void Handle_ValidCommand_ShouldMakeEventPrivate()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Draft)
            .Build();

        var command = MakeEventPrivateCommand.Create(@event.Id.Value).Payload;
        
        var eventRepo = new FakeEventRepo();
        eventRepo._events.Add(@event);

        var uow = new FakeUoW();
        var handler = new MakeEventPrivateHandler(eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Private, @event.EventVisibility);
        Assert.Equal(EventStatus.Draft, @event.EventStatus); // still draft
    }
    
    // UC6.F1
    [Fact]
    public void Handle_EventIsActive_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();
        
        var fakeRepo = new FakeEventRepo();
        fakeRepo._events.Add(@event);
        var uow = new FakeUoW();

        

        var command = MakeEventPrivateCommand.Create(@event.Id.Value).Payload;
        var handler = new MakeEventPrivateHandler(fakeRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.ActiveEventCannotBeMadePrivate, result.Error);
    }
}
