using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Event.SetReadyEvent;

public class SetReadyEventHandlerTest {
    //ID:UC8.S1
    [Fact]
    public async Task PerformAction_SetReadyEventCommand_Success() {
        //Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithValidTitle()
            .WithValidTimeInFuture()
            .Build();

        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = SetReadyEventCommand.Create(@event.Id.Value).Payload;
        var handler = new SetReadyEventHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);


        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, @event.Status);
    }

    //ID:UC8.F1
    [Fact]
    public async Task PerformAction_SetReadyEventCommand_EventTimeSpanIsNotSet_Failure() {
        //Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = SetReadyEventCommand.Create(@event.Id.Value).Payload;
        var handler = new SetReadyEventHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsNotSet, result.Error);
    }
}