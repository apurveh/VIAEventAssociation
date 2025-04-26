using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleHandlerTest {
    // ID:UC2.S1
    [Fact]
    public async Task Handle_ValidInput_Success() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var newTitle = "New Title";
        var command = UpdateTitleCommand.Create(@event.Id.Value, newTitle).Payload;

        var handler = new UpdateTitleHandler(repo, uow);


        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, @event.Title.Value);
    }

    // ID:UC2.F1
    [Fact]
    public async Task Handle_EventId_NotFound_Failure() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var newTitle = "New Title";
        var newEventNotAddedToTheRepo = EventFactory.Init().Build();
        var command = UpdateTitleCommand.Create(newEventNotAddedToTheRepo.Id.Value, newTitle).Payload;

        var handler = new UpdateTitleHandler(repo, uow);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsNotFound, result.Error);
    }
}