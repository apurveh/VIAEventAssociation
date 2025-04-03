namespace UnitTests.Features.EventTests.ReadyEvent;

using Fakes;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Application.Features.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

public class ReadyEventHandlerTest
{
    // UC8 Sunny
    [Fact]
    public void Handle_ValidCommand_ShouldMakeEventReady()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Draft)
            .Build();

        var command = ReadyEventCommand.Create(@event.Id.Value).Payload;

        var eventRepo = new FakeEventRepo();
        eventRepo._events.Add(@event);

        var uow = new FakeUoW();
        var handler = new ReadyEventHandler(eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, @event.EventStatus);
    }

    // UC8 Rainy
    [Fact]
    public void Handle_EventIsCancelled_ShouldReturnFailure()
    {
        // Arrange
        var @event = EventFactory
            .Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Cancelled)
            .Build();

        var command = ReadyEventCommand.Create(@event.Id.Value).Payload;

        var eventRepo = new FakeEventRepo();
        eventRepo._events.Add(@event);

        var uow = new FakeUoW();
        var handler = new ReadyEventHandler(eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.CancelledEventCannotBeModified, result.Error);
    }
}
