using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateCommandTest {
    //ID:UC6.S1
    [Fact]
    public void Create_MakePrivateCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();

        //Act
        var result = MakePrivateCommand.Create(guid);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC5.F1
    [Fact]
    public async Task Handle_MakePrivateCommand_WithEventStatusActive_Failure() {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Active)
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
        Assert.Equal(Error.EventStatusIsActive, result.Error);
    }
}