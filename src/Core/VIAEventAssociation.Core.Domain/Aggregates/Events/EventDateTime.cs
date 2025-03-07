using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDateTime : DateTimeRange
{
    private EventDateTime(DateTime start, DateTime end) : base(start, end) { }

    public static Result<EventDateTime> Create(DateTime start, DateTime end)
    {
        try
        {
            var validation = Validate(start, end);
            return validation.IsSuccess ? new EventDateTime(start, end) : validation.Error;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    private static Result Validate(DateTime start, DateTime end)
    {
        var errors = new HashSet<Error>();
        
        // Validations

        return Result.Ok;
    }
}