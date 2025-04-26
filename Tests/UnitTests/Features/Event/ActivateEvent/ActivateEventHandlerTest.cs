using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using Xunit;

namespace UnitTests.Features.Event.ActivateEvent;

public class ActivateEventHandlerTest {
    //ID:UC9.S1
    [Fact]
    public async void Handle_ActivateEventCommand_WithValidData() {
        //Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithValidTimeInFuture()
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();
        var handler = new ActivateEventHandler(repo, uow);

        var command = ActivateEventCommand.Create(@event.Id.Value).Payload;

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    //ID:UC9.F1
    [Fact]
    public async void Handle_ActivateEventCommand_WithEventStatusCancelled_Failure() {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();
        var handler = new ActivateEventHandler(repo, uow);

        var command = ActivateEventCommand.Create(@event.Id.Value).Payload;

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
    }
}