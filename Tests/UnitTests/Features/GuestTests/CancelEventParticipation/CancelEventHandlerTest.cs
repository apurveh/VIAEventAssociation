using UnitTests.Fakes;
using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Application.Features.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.CancelEventParticipation;

public class CancelEventHandlerTest
{
    // UC12.S1
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

        guest.RegisterToEvent(@event);
        
        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        eventRepo._events.Add(@event);
        
        var uow = new FakeUoW();

        var command = CancelEventParticipationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new CancelEventParticipationHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(@event.IsParticipating(guest));
        Assert.False(guest.IsConfirmedInEvent(@event).Payload);
    }
    
    // UC12.F1
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
        
        var uow = new FakeUoW();

        var command = CancelEventParticipationCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new CancelEventParticipationHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsNotFound, result.Error);
    }
}