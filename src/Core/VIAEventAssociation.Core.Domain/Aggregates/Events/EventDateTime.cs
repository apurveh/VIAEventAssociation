using System.Globalization;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Agregates.Events;

public class EventDateTime : DateTimeRange {
    internal static readonly TimeSpan EARLIEST_START = new(8, 0, 0); // 08:00 AM
    private static readonly TimeSpan LATEST_START = new(0, 0, 0); // 00:00 AM, events cannot start after midnight
    private static readonly TimeSpan LATEST_END = new(1, 0, 0); // 01:00 AM
    private static readonly TimeSpan MAX_DURATION = new(10, 0, 0); // 10 hours
    private static readonly TimeSpan MIN_EVENT_DURATION = new(1, 0, 0); // 30 minutes
    private EventDateTime(DateTime start, DateTime end) : base(start, end) { }

    private EventDateTime() : base(default, default) { } // For EF Core

    public static Result<EventDateTime> Create(DateTime start, DateTime end) {
        try {
            var validation = Validate(start, end);
            return validation.IsSuccess ? new EventDateTime(start, end) : validation.Error;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<EventDateTime> CreateFromStrings(string start, string end) {
        try {
            var startDateTime = DateTime.ParseExact(start, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            var endDateTime = DateTime.ParseExact(end, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            return Create(startDateTime, endDateTime);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result Validate(DateTime start, DateTime end) {
        var errors = new HashSet<Error>();

        // Check if the start time is after the end time.
        if (start >= end)
            errors.Add(Error.InvalidDateTimeRange);

        // Check if the start time is before 08:00 AM and after 00:00 AM (midnight).
        if (start.TimeOfDay < EARLIEST_START && start.TimeOfDay > LATEST_START)
            errors.Add(Error.InvalidStartDateTime(start));

        // check if the event duration is at least MIN_EVENT_DURATION
        if (end - start < MIN_EVENT_DURATION)
            errors.Add(Error.EventTooShort(MIN_EVENT_DURATION));

        // Check if the end time is properly set considering the valid operating hours
        // Validate end time based on the operational rules.
        if (end.Date == start.Date) {
            // If it's the same day, ensure the end is no later than midnight (considered as 23:59:59 for practical purposes),
            // and the start must be after 08:00 AM.
            if (end.TimeOfDay >= LATEST_END && start.TimeOfDay < EARLIEST_START)
                errors.Add(Error.InvalidEndDateTime(end));
        }
        else if (end.Date > start.Date) {
            // If it's the next day, ensure the end time is no later than 01:00 AM.
            if (end.TimeOfDay > LATEST_END) errors.Add(Error.InvalidEndDateTime(end));
        }


        // Ensure the event does not last longer than 10 hours.
        if (end - start > MAX_DURATION)
            errors.Add(Error.InvalidDuration(start, end, MAX_DURATION));


        // Return all collected errors, if any.
        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    public bool Overlaps(DateTimeRange other) {
        return Start < other.End && End > other.Start;
    }
}