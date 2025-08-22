using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuestHandlerTest {
    //ID:UC7.S1
    [Fact]
    public async Task Handle_SetMaxGuestCommand_WithValidData() {
        //Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithValidTimeInFuture()
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();
        var handler = new SetMaxGuestHandler(repo, uow);
        var maxGuests = "10";

        var command = SetMaxGuestCommand.Create(@event.Id.Value, maxGuests).Payload;

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(maxGuests, @event.MaxNumberOfGuests.ToString());
    }

    //ID:UC7.F1
    [Fact]
    public async Task Handle_SetMaxGuestCommand_WithEventStatusCancelled_Failure() {
        //Arrange
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();
        var handler = new SetMaxGuestHandler(repo, uow);
        var maxGuests = "10";

        var command = SetMaxGuestCommand.Create(@event.Id.Value, maxGuests).Payload;

        //Act
        var result = await handler.HandleAsync(command);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
    }
}