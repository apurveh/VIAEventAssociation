using VIAEventAssociation.Core.Domain.Common.Values;

namespace UnitTests.Features.EventTests.SetMaxGuests;

using Fakes;
using VIAEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventAssociation.Core.Application.Features.Event;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

public class SetMaxGuestsHandlerTest
{
    // UC7 Sunny
    [Fact]
    public void Handle_ValidCommand_ShouldSetMaxGuests()
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

        var command = SetMaxGuestsCommand.Create(@event.Id.Value, 30).Payload;

        var eventRepo = new FakeEventRepo();
        eventRepo._events.Add(@event);

        var uow = new FakeUoW();
        var handler = new SetMaxGuestsHandler(eventRepo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(30, @event.MaxNumberOfGuests.Value);
    }
    
    // UC7 Rainy
    [Fact]
    public void Handle_TooFewGuests_ShouldReturnFailure()
    {
        // Arrange
        var eventId = "EID" + Guid.NewGuid();
        var invalidGuestCount = 2;

        // Act
        var commandResult = SetMaxGuestsCommand.Create(eventId, invalidGuestCount);

        // Assert
        Assert.True(commandResult.IsFailure);
        Assert.Contains(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS), commandResult.Error.GetAllErrors());
    }
}