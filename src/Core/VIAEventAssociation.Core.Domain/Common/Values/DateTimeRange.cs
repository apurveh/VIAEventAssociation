using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public class DateTimeRange : ValueObject {
    protected DateTimeRange(DateTime start, DateTime end) {
        Start = start;
        End = end;
    }

    private DateTimeRange() { } // Required by EF Core

    public DateTime Start { get; }
    public DateTime End { get; }

    public static Result<DateTimeRange> Create(DateTime start, DateTime end) {
        try {
            var validation = Validate(start, end);
            if (validation.IsFailure)
                return validation.Error;
            return new DateTimeRange(start, end);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(DateTime start, DateTime end) {
        if (start > end)
            return Error.InvalidDateTimeRange;
        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Start;
        yield return End;
    }

    public static bool isPast(DateTimeRange dateTimeRange) {
        return dateTimeRange.Start < DateTime.Now;
    }

    public static bool isFuture(DateTimeRange dateTimeRange) {
        return dateTimeRange.Start > DateTime.Now;
    }

    public override string ToString() => $"Start: {Start}, End: {End}";
}