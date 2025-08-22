using System.Globalization;
using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;
using ViaEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.UpdateTimeInterval;

public class UpdateTimeIntervalHandlerTest {
    // ID:UC3.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var @event = EventFactory.Init().Build();
        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var guid = @event.Id.Value;
        var start = "2025-01-01T08:20:00";
        var end = "2025-01-01T10:00:00";
        // The expected format for DateTime comparison
        var dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        var command = UpdateTimeIntervalCommand.Create(guid, start, end).Payload;

        var handler = new UpdateTimeIntervalHandler(repo, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Convert DateTime to strings using the expected format for comparison
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, @event.Id.Value);
        Assert.Equal(start, @event.TimeSpan.Start.ToString(dateTimeFormat,CultureInfo.InvariantCulture));
        Assert.Equal(end, @event.TimeSpan.End.ToString(dateTimeFormat,CultureInfo.InvariantCulture));
    }

    // ID:UC3.F1
    [Fact]
    public void Create_EventIsActive_Failure() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        var repo = new InMemEventRepoStub();
        repo._events.Add(@event);
        var uow = new FakeUoW();

        var guid = @event.Id.Value;
        var start = "2025-01-01T08:20:00";
        var end = "2025-01-01T10:00:00";

        var command = UpdateTimeIntervalCommand.Create(guid, start, end).Payload;

        var handler = new UpdateTimeIntervalHandler(repo, uow);

        // Act
        var result = handler.HandleAsync(command);
        var res = result.Result;

        // Assert
        Assert.True(res.IsFailure);
    }
}