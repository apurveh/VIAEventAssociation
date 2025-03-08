using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Common.Values;

public class DateTimeRange : ValueObject
{
    protected DateTimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
    public DateTime Start { get; }
    public DateTime End { get; }

    public static Result<DateTimeRange> Create(DateTime start, DateTime end)
    {
        try
        {
            var validation = Validate(start, end);
            if (validation.IsFailure)
                return validation.Error;
            return new DateTimeRange(start, end);
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
    
    private static Result Validate(DateTime start, DateTime end)
    {
        if (start > end)
            return Error.InvalidDateTimeRange;
        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
    
    public static bool IsPast(DateTimeRange dateTimeRange)
    {
        return dateTimeRange.Start < DateTime.Now;
    }
    
    public static bool IsFuture(DateTimeRange dateTimeRange)
    {
        return dateTimeRange.Start > DateTime.Now;
    }

    public override string ToString() => $"Start: {Start}, End: {End}";
}