using UnitTests.Fakes;
using UnitTests.Features.GuestTests;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Application.Features.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.EventTests.InviteGuest;

public class InviteGuestHandlerTest
{
    // UC13.S1
    [Fact]
    public void Handle_ValidInput_Success() {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();
        
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .Build();

        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        eventRepo._events.Add(@event);
        var uow = new FakeUoW();

        var command = InviteGuestCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new InviteGuestHandler(guestRepo, eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(guest.IsPendingInEvent(@event).Payload);
    }

    // UC13.F1
    [Fact]
    public void Handle_EventNotFound_Failure() {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();
        
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .Build();

        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        
        var uow = new FakeUoW();

        var command = InviteGuestCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new InviteGuestHandler(guestRepo, eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsNotFound, result.Error);
    }
}