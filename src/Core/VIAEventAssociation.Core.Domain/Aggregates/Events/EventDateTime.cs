using System;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDateTime : DateTimeRange
{
    private EventDateTime(DateTime start, DateTime end) : base(start, end) { }

    public static Result<EventDateTime> Create(DateTime start, DateTime end)
    {
        var validation = Validate(start, end);
        
        return validation.IsSuccess ? new EventDateTime(start, end) : validation.Error;
    }

    private static Result Validate(DateTime start, DateTime end)
    {
        var errors = new HashSet<Error>();
        
        TimeSpan duration = end - start;
        if (duration.TotalHours < 1 || duration.TotalHours > 10)
        {
            errors.Add(Error.InvalidDateTimeRange);
        }
        else
        {
            if (start >= end)
                errors.Add(Error.InvalidDateTimeRange);

            if (start.Hour < 8)
                errors.Add(Error.InvalidStartDateTime(start));

            if ((start.Hour < 8 && end.Hour < 1))
                errors.Add(Error.InvalidStartDateTime(start));
        
            if (end.Hour >= 1 && end.Hour < 8)
                errors.Add(Error.InvalidEndDateTime(end));
        }

        return errors.Count > 0 ? Error.Add(errors) : Result.Ok;
    }
    
    public static bool IsPastEvent(DateTime end)
    {
        return end < DateTime.Now;
    }
}