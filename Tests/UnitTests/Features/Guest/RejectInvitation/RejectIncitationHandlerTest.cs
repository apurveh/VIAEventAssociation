using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Guest;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Guest.RejectInvitation;

public class RejectIncitationHandlerTest {
    // ID:UC15.S1
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
        @event.SendInvitation(guest);

        var command = RejectInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RejectInvitationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(@event.IsParticipating(guest));
        Assert.False(guest.IsConfirmedInEvent(@event).Payload);
    }

    // ID:UC15.F1
    [Fact]
    public void Handle_WithStatusCanceled_ShouldReturnError() {
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
        @event.SendInvitation(guest);
        @event.CancelEvent();
        var command = RejectInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RejectInvitationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceledAndCannotRejectInvitation, result.Error.GetAllErrors());
    }
}