using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Guest.CancelEventParticipation;

public class CancelEventParticipationHandlerTest {
    // ID:UC12.S1
    [Fact]
    public void Handle_ValidInput_Success() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();

        var repo = new InMemGuestRespoStub();
        var eventRepo = new InMemEventRepoStub();
        repo._guests.Add(guest);
        eventRepo._events.Add(@event);
        var uow = new FakeUoW();

        var command = CancelEventParticipationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new CancelEventParticipationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(@event.IsParticipating(guest));
        Assert.False(guest.IsConfirmedInEvent(@event).Payload);
    }

    // ID:UC12.F1
    [Fact]
    public void Handle_EventNotFound_Failure() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .Build();

        var guest = GuestFactory.InitWithDefaultsValues().Build();

        var repo = new InMemGuestRespoStub();
        var eventRepo = new InMemEventRepoStub();
        repo._guests.Add(guest);
        //eventRepo._events.Add(@event);
        //EVENT DOES NOT EXITST
        var uow = new FakeUoW();

        var command = CancelEventParticipationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new CancelEventParticipationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
    }
}