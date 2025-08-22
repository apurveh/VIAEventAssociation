using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Guest.AcceptInvitation;

public class AcceptsInvitationHandlerTest {
    // ID:UC14.S1
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

        var command = AcceptsInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new AcceptsInvitationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(@event.IsParticipating(guest));
        Assert.True(guest.IsConfirmedInEvent(@event).Payload);
    }

    // ID:UC14.F1
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

        var command = AcceptsInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new AcceptsInvitationHandler(repo, uow, eventRepo);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsNotActive, result.Error.GetAllErrors());
    }
}