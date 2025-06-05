using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuests {
    // Given an existing event with ID, and the event status is draft or ready, when creator sets the maximum number of guests, then the maximum number of guests is set to the selected value
    // ID:UC7.S1
    [Theory]
    [InlineData(5, EventStatus.Draft)]
    [InlineData(5, EventStatus.Ready)]
    [InlineData(10, EventStatus.Draft)]
    [InlineData(10, EventStatus.Ready)]
    [InlineData(25, EventStatus.Draft)]
    [InlineData(25, EventStatus.Ready)]
    [InlineData(50, EventStatus.Draft)]
    [InlineData(50, EventStatus.Ready)]
    public void SetMaxGuests_EventInDraftOrReadyStatus_MaxGuestsSet(int maxGuests, EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxNumberOfGuests.Value);
    }

    // Given an existing event with ID, and the event status is draft or ready, when creator sets the maximum number of guests, then the maximum number of guests is set to the selected value
    // ID:UC7.S2
    [Theory]
    [InlineData(5, EventStatus.Draft)]
    [InlineData(5, EventStatus.Ready)]
    [InlineData(10, EventStatus.Draft)]
    [InlineData(10, EventStatus.Ready)]
    [InlineData(25, EventStatus.Draft)]
    [InlineData(25, EventStatus.Ready)]
    [InlineData(50, EventStatus.Draft)]
    [InlineData(50, EventStatus.Ready)]
    public void SetMaxGuests_EventInDraftOrReadyStatus_MaxGuestsSet2(int maxGuests, EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxNumberOfGuests.Value);
    }

    // Given an existing event with ID, and the event is in active status, when creator sets the maximum number of guests, then the maximum number of guests is set to the selected value
    // ID:UC7.S3
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxGuests_EventInActiveStatus_MaxGuestsSet(int maxGuests) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxNumberOfGuests.Value);
    }

    // Given an existing event with ID, and the event is in active status, when creator reduces the number of maximum guests, then a failure message is provided explaining the maximum number of guests of an active cannot be reduced (it may only be increased)
    // ID:UC7.F1
    [Fact]
    public void SetMaxGuests_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(25)
            .Build();
        var maxGuests = 10;

        // Act
        var result = @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActiveAndMaxGuestsReduced.Message, result.Error.Message);
    }

    // Given an existing event with ID, and the event is in cancelled status, when creator sets the number of maximum guests, then a failure message is provided explaining a cancelled event cannot be modified
    // ID:UC7.F2
    [Fact]
    public void SetMaxGuests_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.SetMaxGuests(25);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }

    // Given an existing event with ID, and the event has a location (see UC16-20), when creator sets the maximum number of guests, then the request is rejected, with a message explaining you cannot have more people at an event than there is room for.
    // ID:UC7.F3
    // [Fact]
    // public void SetMaxGuests_MaxGuestsLargerThanLocationMax_FailureMessageReturned()
    // {
    //     // Arrange
    //     var @event = EventFactory.Init()
    //                              .WithStatus(EventStatus.Draft)
    //                              .WithLocation(LocationFactory.Init()
    //                                                        .WithMaxNumberOfPeople(10)
    //                                                        .Build())
    //                              .Build();
    //
    //     // Act
    //     var result = @event.SetMaxGuests(25);
    //
    //     // Assert
    //     Assert.True(result.IsFailure);
    //     Assert.Contains(Error.TooManyGuests.Message, result.Error.Message);
    // }

    // Given an existing event with ID, when creator sets the number of maximum guests to number < 5, then a failure message is provided explaining the maximum number of guests cannot be negative
    // ID:UC7.F4
    [Fact]
    public void SetMaxGuests_MaxGuestsLessThan5_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var newMaxGuests = 4;

        // Act
        var result = @event.SetMaxGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS).Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator sets the number of maximum guests to a number > 50, then a failure message is provided explaining the maximum number of guests cannot exceed 50
    // ID:UC7.F5
    [Fact]
    public void SetMaxGuests_MaxGuestsMoreThan50_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var newMaxGuests = 51;

        // Act
        var result = @event.SetMaxGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS).Message, result.Error.Message);
    }
}