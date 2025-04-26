using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateHandlerTest {
    //ID:UC6.S1
    [Fact]
    public async Task Handle_MakePrivateCommand_WithValidData() {
        //Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = MakePrivateCommand.Create(@event.Id.Value).Payload;

        var handler = new MakePrivateHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(@event.Visibility == EventVisibility.Private);
    }

    //ID:UC6.F1
    [Fact]
    public async Task Handle_MakePrivateCommand_WithEventStatusCancelled_Failure() {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = MakePrivateCommand.Create(@event.Id.Value).Payload;

        var handler = new MakePrivateHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
    }
}