using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Guest.GuestRequestToJoin;

public class GuestParticipatePublicHandlerTest {
    //ID:UC11.S1
    [Fact]
    public void Handle_ValidInput_Success() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues().Build();
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        var repo = new InMemGuestRespoStub();
        var eventRepo = new InMemEventRepoStub();
        repo._guests.Add(guest);
        eventRepo._events.Add(@event);
        var uow = new FakeUoW();

        var command = RequestToJoinCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RequestToJoinHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(@event.Participations.Count, 1);
        Assert.Equal(guest.Participations.Count, 1);
    }

    //ID:UC11.F1
    [Fact]
    public void Handle_InvalidEventId_Failure() {
        // Arrange
        var guest = GuestFactory.InitWithDefaultsValues().Build();
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        var repo = new InMemGuestRespoStub();
        var eventRepo = new InMemEventRepoStub();
        repo._guests.Add(guest);
        //eventRepo._events.Add(@event);
        //EVENT DOES NOT EXITST
        var uow = new FakeUoW();

        var command = RequestToJoinCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RequestToJoinHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventIsNotFound, result.Error.GetAllErrors());
    }
}