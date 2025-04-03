namespace UnitTests.Features.EventTests.ActivateEvent
{
    using Fakes;
    using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
    using VIAEventAssociation.Core.Application.Features.Event;
    using VIAEventAssociation.Core.Domain.Aggregates.Events;
    using VIAEventAssociation.Core.Tools.OperationResult;
    using Xunit;

    public class ActivateEventHandlerTest
    {
        // UC9 Sunny
        [Fact]
        public void Handle_ValidCommand_ShouldActivateEvent()
        {
            // Arrange
            var @event = EventFactory
                .Init()
                .WithValidTitle()
                .WithValidDescription()
                .WithValidTimeInFuture()
                .WithStatus(EventStatus.Ready)
                .WithVisibility(EventVisibility.Public)
                .Build();

            var command = ActivateEventCommand.Create(@event.Id.Value).Payload;

            var eventRepo = new FakeEventRepo();
            eventRepo._events.Add(@event);

            var uow = new FakeUoW();
            var handler = new ActivateEventHandler(eventRepo, uow);

            // Act
            var result = handler.HandleAsync(command).Result;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventStatus.Active, @event.EventStatus);
        }

        // UC9 Rainy
        [Fact]
        public void Handle_EventIsNotReady_ShouldReturnFailure()
        {
            // Arrange
            var @event = EventFactory
                .Init()
                .WithValidTitle()
                .WithValidDescription()
                .WithValidTimeInFuture()
                .WithStatus(EventStatus.Cancelled)
                .WithVisibility(EventVisibility.Public)
                .Build();

            var command = ActivateEventCommand.Create(@event.Id.Value).Payload;

            var fakeRepo = new FakeEventRepo();
            fakeRepo._events.Add(@event);

            var uow = new FakeUoW();
            var handler = new ActivateEventHandler(fakeRepo, uow);

            // Act
            var result = handler.HandleAsync(command).Result;

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.CancelledEventCannotBeModified, result.Error);
        }
    }
}
