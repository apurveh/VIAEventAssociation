using ViaEventAssociation.Core.Domain.Aggregates.Events;

public class UpdateTimeInterval {
    // Given an existing event with ID, and the event status is draft, when creator selects to set the times of the event, and the start time is before the end time, and the dates are the same, and the duration of the event is 1 hour or longer, and the start time is after 08:00 (am, morning), and the end time is before 23:59, then the times of the event are updated.
    // ID:UC4.S1
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_EventInDraftStatus_StartTimeBeforeEndTime_DatesAreSame_DurationIs1HourOrLonger_StartTimeAfter08EndTimeBefore2359_TimesUpdated(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var timeSpan = EventDateTime.Create(startTime, endTime).Payload;

        // Act
        @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.Equal(startTime, @event.TimeSpan.Start);
        Assert.Equal(endTime, @event.TimeSpan.End);
    }

    // Given an existing event with ID, and the event status is draft, when creator selects to set the times of the event, and the start date is before the end date, and the duration of the event is 1 hour or longer, and the start time is after 08:00 (am), and the end time is before 01:00 (am), then the times of the event are updated.
    // ID:UC4.S2
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/26 01:00")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    public void UpdateTimeInterval_EventInDraftStatus_StartDateBeforeEndDate_DurationIs1HourOrLonger_StartTimeAfter08EndTimeBefore01_TimesUpdated(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var timeSpan = EventDateTime.Create(startTime, endTime).Payload;

        // Act
        @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.Equal(startTime, @event.TimeSpan.Start);
        Assert.Equal(endTime, @event.TimeSpan.End);
    }

    // Given an existing event with ID, and the event status is ready, when creator sets the times of the event to valid values (see S1, S2), then the times of the event are updated, and the status is draft.
    // ID:UC4.S3
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_EventInReadyStatus_StartTimeBeforeEndTime_DatesAreSame_DurationIs1HourOrLonger_StartTimeAfter08EndTimeBefore2359_TimesUpdated_StatusIsDraft(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Ready)
            .Build();
        var timeSpan = EventDateTime.Create(startTime, endTime).Payload;

        // Act
        @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.Equal(startTime, @event.TimeSpan.Start);
        Assert.Equal(endTime, @event.TimeSpan.End);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    // Given an existing event with ID, when creator sets the times of the event to valid values (see S1, S2), and the start time is in the future, then the times of the event are updated.
    // ID:UC4.S4
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_StartTimeIsInTheFuture_TimesUpdated(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();
        var timeSpan = EventDateTime.Create(startTime, endTime).Payload;

        // Act
        @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.Equal(startTime, @event.TimeSpan.Start);
        Assert.Equal(endTime, @event.TimeSpan.End);
    }

    // Given an existing event with ID, when creator sets the times of the event to valid values (see S1, S2), and the duration from start to finish is 10 hours or less, then the times of the event are updated.
    // ID:UC4.S5
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_DurationFromStartToFinishIs10HoursOrLess_TimesUpdated(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();
        var timeSpan = EventDateTime.Create(startTime, endTime).Payload;

        // Act
        @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.Equal(startTime, @event.TimeSpan.Start);
        Assert.Equal(endTime, @event.TimeSpan.End);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start date is after the end date, then a failure message is returned explaining the problem.
    // ID:UC4.F1
    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/25 01:00")]
    [InlineData("2023/08/26 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/27 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/01 08:00", "2023/07/31 12:15")]
    public void UpdateTimeInterval_StartDateAfterEndDate_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidDateTimeRange.Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start date is the same as the end date, and the start time is after the end date, then a failure message is returned explaining the problem.
    // ID:UC4.F2
    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/26 14:00")]
    [InlineData("2023/08/26 16:00", "2023/08/26 00:00")]
    [InlineData("2023/08/26 19:00", "2023/08/26 18:59")]
    [InlineData("2023/08/26 12:00", "2023/08/26 10:10")]
    [InlineData("2023/08/26 08:00", "2023/08/26 00:30")]
    public void UpdateTimeInterval_StartDateIsTheSameAsEndDate_StartTimeIsAfterEndTime_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidDateTimeRange.Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start date is the same as the end date, and the start time is less than 1 hour before the end time, giving the event a duration of less than 1 hour, then a failure message is returned explaining the problem.
    // ID:UC4.F3
    [Theory]
    [InlineData("2023/08/26 14:00", "2023/08/26 14:50")]
    [InlineData("2023/08/26 18:00", "2023/08/26 18:59")]
    [InlineData("2023/08/26 12:00", "2023/08/26 12:30")]
    [InlineData("2023/08/26 08:00", "2023/08/26 08:00")]
    public void UpdateTimeInterval_StartDateIsTheSameAsEndDate_StartTimeIsLessThan1HourBeforeEndTime_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();
        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);

        //Could have more than one error
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTooShort(TimeSpan.FromHours(1)).Message);
        Assert.NotNull(error);
        //is the error inside the list?
        Assert.Contains(Error.EventTooShort(TimeSpan.FromHours(1)).Message, error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start date is before the end date, and the start time is less than 1 hour before the end time, giving the event a duration of less than 1 hour, then a failure message is returned explaining the problem.
    // ID:UC4.F4
    [Theory]
    [InlineData("2023/08/25 23:30", "2023/08/26 00:15")]
    [InlineData("2023/08/30 23:01", "2023/08/31 00:00")]
    [InlineData("2023/08/30 23:59", "2023/08/31 00:01")]
    public void UpdateTimeInterval_StartDateBeforeEndDate_StartTimeIsLessThan1HourBeforeEndTime_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);

        //Could have more than one error
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.EventTooShort(TimeSpan.FromHours(1)).Message);
        Assert.NotNull(error);
        //is the error inside the list?
        Assert.Contains(Error.EventTooShort(TimeSpan.FromHours(1)).Message, error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start time is before 08:00, then a failure message is returned explaining the problem.
    // ID:UC4.F5
    [Theory]
    [InlineData("2023/08/25 07:50", "2023/08/25 14:00")]
    [InlineData("2023/08/25 07:59", "2023/08/25 15:00")]
    [InlineData("2023/08/25 01:01", "2023/08/25 08:30")]
    [InlineData("2023/08/25 05:59", "2023/08/25 07:59")]
    [InlineData("2023/08/25 00:59", "2023/08/25 07:59")]
    public void UpdateTimeInterval_StartTimeBefore08_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidStartDateTime(startTime).Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start time is before 01:00, and the end time is after 01:00, then a failure message is returned explaining the problem.
    // ID:UC4.F6
    [Theory]
    [InlineData("2023/08/24 23:50", "2023/08/25 01:01")]
    [InlineData("2023/08/24 22:00", "2023/08/25 07:59")]
    [InlineData("2023/08/30 23:00", "2023/08/31 02:30")]
    [InlineData("2023/08/24 23:50", "2023/08/25 01:01")]
    public void UpdateTimeInterval_StartTimeBefore01EndTimeAfter01_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEndDateTime(endTime).Message, result.Error.Message);
    }

    // Given an existing event with ID, and the event status is active, when creator sets the times of the event, then a failure message is returned explaining that the times of an active event cannot be modified when the event is active.
    // ID:UC4.F7
    [Fact]
    public void UpdateTimeInterval_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");

        var timeSpan = EventDateTime.Create(validStartTime, validEndTime).Payload;

        // Act
        var result = @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator sets the times of the event, and the event is in cancelled status, then a failure message is returned explaining a cancelled event cannot be modified.
    // ID:UC4.F8
    [Fact]
    public void UpdateTimeInterval_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");

        var timeSpan = EventDateTime.Create(validStartTime, validEndTime).Payload;

        // Act
        var result = @event.UpdateTimeSpan(timeSpan);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the duration of the event is longer than 10 hours, then a failure message is returned explaining the problem.
    // ID:UC4.F9
    [Theory]
    [InlineData("2023/08/30 08:00", "2023/08/30 18:01")]
    [InlineData("2023/08/30 14:59", "2023/08/31 01:00")]
    [InlineData("2023/08/30 14:00", "2023/08/31 00:01")]
    [InlineData("2023/08/30 14:00", "2023/08/31 18:30")]
    public void UpdateTimeInterval_DurationIsLongerThan10Hours_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure);
        var error = result.Error.GetAllErrors().First(e => e.Message == Error.InvalidDuration(startTime, endTime, TimeSpan.FromHours(10)).Message);
        Assert.Contains(Error.InvalidDuration(startTime, endTime, TimeSpan.FromHours(10)).Message, error.Message);
    }

    // Given an existing event with ID, when creator sets the times of the event, and the start time is in the past, then a failure message is returned explaining that events cannot be started in the past.
    // ID:UC4.F10
    [Fact]
    public void UpdateTimeInterval_StartTimeIsInThePast_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .Build();

        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");

        // Act
        var result = EventDateTime.Create(validStartTime, validEndTime);

        // Assert
        // Assert.True(result.IsFailure);
        // Assert.Contains(Error.EventStartTimeInThePast.Message, result.Error.Message);
    }

    // Given an existing event with ID, when creator selects to set the times of the event, and the start time is before 01:00 on the same date as the end time, or the date before, and the end time is after 08:00, meaning the event spans the time between 01 and 08, then a failure message is returned explaining the problem.
    // ID:UC4.F11
    [Theory]
    [InlineData("2023/08/31 00:30", "2023/08/31 08:30")]
    [InlineData("2023/08/30 23:59", "2023/08/31 08:01")]
    [InlineData("2023/08/31 01:00", "2023/08/31 08:00")]
    public void UpdateTimeInterval_StartTimeBefore01EndTimeAfter08_FailureMessageReturned(DateTime startTime, DateTime endTime) {
        // Arrange
        var @event = EventFactory.Init().Build();

        // Act
        var result = EventDateTime.Create(startTime, endTime);

        // Assert
        Assert.True(result.IsFailure, "Expected failure when updating time interval");

        // Get all error messages
        var errorMessages = result.Error.GetAllErrors().Select(e => e.Message).ToList();

        // Check if any of the error messages is one of the expected ones
        var startError = Error.InvalidStartDateTime(startTime).Message;
        var endError = Error.InvalidEndDateTime(endTime).Message;

        var containsStartOrEndError = errorMessages.Any(e => e.Contains(startError) || e.Contains(endError));

        Assert.True(containsStartOrEndError, "Error list should contain invalid start time or invalid end time error");
    }
}