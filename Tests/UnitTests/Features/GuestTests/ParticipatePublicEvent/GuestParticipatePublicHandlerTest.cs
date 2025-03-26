using UnitTests.Fakes;
using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventAssociation.Core.Application.Features.Guest;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.GuestTests.ParticipatePublicEvent;

public class GuestParticipatePublicHandlerTest
{
    // UC11.S1
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

        var guestRepo = new FakeGuestRepo();
        var eventRepo = new FakeEventRepo();
        guestRepo._guests.Add(guest);
        eventRepo._events.Add(@event);
        var uow = new FakeUoW();
        
        var command = RequestToJoinCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RequestToJoinHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(@event.Participations.Count, 1);
        Assert.Equal(guest.Participations.Count, 1);
    }
    
    // UC11.F1
    [Fact]
    public void Handle_InvalidEventId_ShouldReturnError()
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
        
        var command = RequestToJoinCommand.Create(@event.Id.Value, guest.Id.Value).Payload;
        var handler = new RequestToJoinHandler(guestRepo, eventRepo, uow);
        
        // Act
        var result = handler.HandleAsync(command).Result;
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsNotFound, result.Error);
    }
}