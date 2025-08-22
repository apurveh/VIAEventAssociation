using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuestHandlerTest {
    // ID:UC13.S1
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

        var command = InviteGuestCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new InviteGuestHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(@event.IsInvitedButNotConfirmed(guest));
        Assert.True(guest.IsPendingInEvent(@event).Payload);
    }

    // ID:UC13.F1
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

        var command = InviteGuestCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new InviteGuestHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
    }
}