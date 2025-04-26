using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public class NumberOfGuests : ValueObject {
    private NumberOfGuests(int value) {
        Value = value;
    }

    public int Value { get; }

    public static Result<NumberOfGuests> Create(int numberOfGuests) {
        try {
            var validation = Validate(numberOfGuests);
            if (validation.IsFailure)
                return validation.Error;
            return new NumberOfGuests(numberOfGuests);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(int numberOfGuests) {
        if (numberOfGuests < CONST.MIN_NUMBER_OF_GUESTS)
            return Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS);
        if (numberOfGuests > CONST.MAX_NUMBER_OF_GUESTS)
            return Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS);
        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}