using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionHandlerTest {
    // ID:UC3.S1
    [Fact]
    public async Task Handle_ValidInput_Success() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var newDescription = "New Description";
        var command = UpdateDescriptionCommand.Create(@event.Id.Value, newDescription).Payload;

        var handler = new UpdateDescriptionHandler(repo, uow);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newDescription, @event.Description.Value);
    }

    // ID:UC3.F1
    [Fact]
    public async Task Handle_EventId_NotFound_Failure() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var newDescription = "New Description";
        var newEventNotAddedToTheRepo = EventFactory.Init().Build();
        var command = UpdateDescriptionCommand.Create(newEventNotAddedToTheRepo.Id.Value, newDescription).Payload;

        var handler = new UpdateDescriptionHandler(repo, uow);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsNotFound, result.Error);
    }
}