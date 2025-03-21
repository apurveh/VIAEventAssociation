using UnitTests.Features.EventTests;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.EventTests.UpdateEventTimeTests;

public class UpdateEventTimeTests
{
    // UC4.S1 - Successfully update times (Draft status, same date)
    [Fact]
    public void UpdateEventTime_WithValidTimesInDraftStatus_ShouldReturnSuccess()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

         var startTime = new DateTime(2028, 5, 25, 10, 0, 0);
         var endTime = new DateTime(2028, 5, 25, 20, 0, 0);
         var result = @event.UpdateTime(startTime, endTime);
        

        Assert.True(result.IsSuccess, $"Expected success but got failure: {result.Error?.Message}");
        Assert.Equal(startTime, @event.EventTime.Start);
        Assert.Equal(endTime, @event.EventTime.End);
    }


     // UC4.S2 - Successfully update times (Draft status, next day end)
     [Fact]
     public void UpdateEventTime_WithValidTimesNextDay_ShouldReturnSuccess()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var tomorrow = DateTime.UtcNow.Date.AddDays(1);
         var startTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0);
         var endTime = startTime.AddHours(1); 
         var result = @event.UpdateTime(startTime, endTime);
         
         Assert.True(result.IsSuccess);
         Assert.Equal(startTime, @event.EventTime.Start);
         Assert.Equal(endTime, @event.EventTime.End);
     }



     // UC4.S3 - Successfully update times when event is in Ready status (changes to Draft)
     [Fact]
     public void UpdateEventTime_WithValidTimesInReadyStatus_ShouldChangeToDraft()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Ready)
             .Build();

         var startTime = new DateTime(2028, 5, 25, 10, 0, 0);
         var endTime = new DateTime(2028, 5, 25, 20, 0, 0);
         var result = @event.UpdateTime(startTime, endTime);

         Assert.True(result.IsSuccess);
         Assert.Equal(startTime, @event.EventTime.Start);
         Assert.Equal(endTime, @event.EventTime.End);
         Assert.Equal(EventStatus.Draft, @event.EventStatus);
     }

     // UC4.S4 - Successfully update future start times
     [Fact]
     public void UpdateEventTime_WithFutureStartTime_ShouldReturnSuccess()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2025, 9, 1, 12, 0, 0);
         var endTime = new DateTime(2025, 9, 1, 16, 0, 0);
         var result = @event.UpdateTime(startTime, endTime);

         Assert.True(result.IsSuccess);
         Assert.Equal(startTime, @event.EventTime.Start);
         Assert.Equal(endTime, @event.EventTime.End);
     }

     // UC4.S5 - Successfully update times within valid duration (1-10 hours)
     [Fact]
     public void UpdateEventTime_WithValidDuration_ShouldReturnSuccess()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2026, 8, 25, 13, 0, 0);
         var endTime = new DateTime(2026, 8, 25, 22, 0, 0);
         var result = @event.UpdateTime(startTime, endTime);

         Assert.True(result.IsSuccess);
         Assert.Equal(startTime, @event.EventTime.Start);
         Assert.Equal(endTime, @event.EventTime.End);
     }
     
     
     // UC4.F1 - Start date is after end date
     [Fact]
     public void UpdateEventTime_WithStartDateAfterEndDate_ShouldReturnFailure()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2023, 8, 26, 19, 0, 0);
         var endTime = new DateTime(2023, 8, 25, 1, 0, 0);
         var result = @event.UpdateTime(startTime, endTime);

         Assert.False(result.IsSuccess);
         Assert.Equal(Error.InvalidDateTimeRange, result.Error);
     }

     [Fact]
     public void UpdateEventTime_WithStartDateAfterEndDate_ShouldReturnFailure_Case2()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2023, 8, 26, 19, 0, 0);
         var endTime = new DateTime(2023, 8, 25, 23, 59, 0);
         var result = @event.UpdateTime(startTime, endTime);

         Assert.False(result.IsSuccess);
         Assert.Equal(Error.InvalidDateTimeRange, result.Error);
     }
     
     [Fact]
     public void UpdateEventTime_WithStartTimeAfterEndTime_ShouldReturnFailure()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2023, 8, 26, 19, 0, 0);
         var endTime = new DateTime(2023, 8, 26, 14, 0, 0); // Invalid (start time is after end time)
         var result = @event.UpdateTime(startTime, endTime);

         Assert.False(result.IsSuccess);
         Assert.Equal(Error.InvalidDateTimeRange, result.Error);
     }

     [Fact]
     public void UpdateEventTime_WithStartTimeAfterEndTime_ShouldReturnFailure_Case2()
     {
         var @event = EventFactory
             .Init()
             .WithStatus(EventStatus.Draft)
             .Build();

         var startTime = new DateTime(2023, 8, 26, 16, 0, 0);
         var endTime = new DateTime(2023, 8, 26, 10, 0, 0); // Invalid (start time is after end time)
         var result = @event.UpdateTime(startTime, endTime);

         Assert.False(result.IsSuccess);
         Assert.Equal(Error.InvalidDateTimeRange, result.Error);
     }
     
      // F3 - Event duration too short (less than 1 hour)
    [Fact]
    public void UpdateEventTime_WithDurationTooShort_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2025, 8, 26, 8, 0, 0);
        var endTime = new DateTime(2025, 8, 26, 8, 50, 0); // Less than 1 hour
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidDateTimeRange, result.Error);
    }

    // F4 - Event duration too short when spanning two dates
    [Fact]
    public void UpdateEventTime_WithDurationTooShortAcrossDates_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2026, 8, 25, 23, 30, 0);
        var endTime = new DateTime(2026, 8, 26, 0, 15, 0); // Less than 1 hour
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidDateTimeRange, result.Error);
    }

    // F5 - Event start time is too early (before 08:00)
    [Fact]
    public void UpdateEventTime_WithStartTimeTooEarly_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2026, 8, 25, 7, 50, 0); // Before 08:00
        var endTime = new DateTime(2026, 8, 25, 9, 0, 0);
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidStartDateTime(startTime), result.Error);
    }

    // F6 - Event end time is too late (after 01:00)
    [Fact]
    public void UpdateEventTime_WithEndTimeTooLate_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2025, 8, 24, 23, 50, 0);
        var endTime = new DateTime(2025, 8, 25, 1, 10, 0); // After 01:00
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidEndDateTime(endTime), result.Error);
    }

    // F7 - Event is active and cannot be modified
    [Fact]
    public void UpdateEventTime_WhenEventIsActive_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Active) // Event is active
            .Build();

        var startTime = new DateTime(2023, 8, 25, 10, 0, 0);
        var endTime = new DateTime(2023, 8, 25, 12, 0, 0);
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.EventStatusIsActive, result.Error);
    }

    // F8 - Event is cancelled and cannot be modified
    [Fact]
    public void UpdateEventTime_WhenEventIsCancelled_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Cancelled) // Event is cancelled
            .Build();

        var startTime = new DateTime(2023, 8, 25, 10, 0, 0);
        var endTime = new DateTime(2023, 8, 25, 12, 0, 0);
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.EventStatusIsCanceled, result.Error);
    }
    
    // F9 - Event duration too long (more than 10 hours)
    [Fact]
    public void UpdateEventTime_WithDurationTooLong_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2025, 8, 30, 8, 0, 0);
        var endTime = new DateTime(2025, 8, 30, 18, 1, 0); // More than 10 hours
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidDateTimeRange, result.Error);
    }

    // F10 - Event start time is in the past
    [Fact]
    public void UpdateEventTime_WithStartTimeInPast_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var past = new DateTime(2023, 8, 30, 8, 0, 0);
        var startTime = past;
        var endTime = startTime.AddHours(2);
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.StartTimeIsInThePast, result.Error);
    }

    // F11 - Event duration spans invalid time (between 01:00 - 08:00)
    [Fact]
    public void UpdateEventTime_WithInvalidTimeSpan_ShouldReturnFailure()
    {
        var @event = EventFactory
            .Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var startTime = new DateTime(2025, 8, 31, 0, 30, 0); // Starts before 01:00
        var endTime = new DateTime(2025, 9, 1, 7, 30, 0);   // Ends after 08:00
        var result = @event.UpdateTime(startTime, endTime);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InvalidDateTimeRange, result.Error);
    }
}
