using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Services;
using Xunit.Abstractions;

namespace UnitTests.Features.Event.SetLocation;

public class SetLocation {
    private readonly ITestOutputHelper _testOutputHelper;

    public SetLocation(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    //	Event creator sets location of event
    //ID:UC20.S1
    [Fact]
    public void SetLocation_ValidLocation_LocationSet() {
        // Arrange
        var @event = EventFactory.Init()
            .WithVisibility(EventVisibility.Public)
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .Build();

        var location = LocationFactory.Init()
            .WithAvailableTimeSpan(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10))
            .Build();

        // Act
        var result = AddLocationToEvent.Handle(@event, location);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(@event, location.Events);
    }

    //	Event creator sets location of event to a location that is already associated with another event
    //ID:UC20.F2
    [Fact]
    public void SetLocation_LocationAlreadyAssociatedWithAnotherEvent_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();

        var event2 = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();

        var location = LocationFactory
            .Init()
            .WithAvailableTimeSpan(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10))
            .Build();

        AddLocationToEvent.Handle(event2, location);

        // Act
        var result = AddLocationToEvent.Handle(@event, location);

        // Assert
        Assert.True(result.IsFailure);
        _testOutputHelper.WriteLine(result.Error.Message);
    }

    //23)	Event creator sets location of event to a location that is not available during the event time
    //ID:UC20.F3
    [Fact]
    public void SetLocation_LocationNotAvailableDuringEventTime_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithVisibility(EventVisibility.Public)
            .WithStatus(EventStatus.Active)
            .Build();

        var location = LocationFactory.Init()
            .WithAvailableTimeSpan(DateTime.Now.AddYears(-10), DateTime.Now)
            .Build();

        // Act
        var result = AddLocationToEvent.Handle(@event, location);

        // Assert
        Assert.True(result.IsFailure);
        _testOutputHelper.WriteLine(result.Error.Message);
    }
}