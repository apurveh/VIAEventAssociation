using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePublicHandlerTest {
    //ID:UC5.S1
    [Fact]
    public async Task Handle_MakePublicCommand_WithValidData() {
        //Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = MakePublicCommand.Create(@event.Id.Value).Payload;

        var handler = new MakePublicHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(@event.Visibility == EventVisibility.Public);
    }

    //ID:UC5.F1
    [Fact]
    public async Task Handle_MakePublicCommand_WithEventStatusCancelled_Failure() {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var command = MakePublicCommand.Create(@event.Id.Value).Payload;

        var handler = new MakePublicHandler(repo, uow);

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
    }
}