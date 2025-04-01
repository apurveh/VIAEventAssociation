using UnitTests.Fakes;
using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Application.Features.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.AcceptInvitation;

public class AcceptInvitationHandlerTest
{
    // UC14.S1
    [Fact]
    public void Handle_ValidInput_Success()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        @event.SendInvitation(guest);

        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        eventRepo._events.Add(@event);
        
        var uow = new FakeUoW();
        
        var command = AcceptInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new AcceptInvitationHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(@event.IsParticipating(guest));
        Assert.True(guest.IsConfirmedInEvent(@event).Payload);
    }
    
    // UC14.F1
    [Fact]
    public void Handle_EventNotFound_Failure()
    {
        // Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        eventRepo._events.Add(@event);
        
        var uow = new FakeUoW();
        
        var command = AcceptInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new AcceptInvitationHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvitationPendingNotFound, result.Error);
    }
}