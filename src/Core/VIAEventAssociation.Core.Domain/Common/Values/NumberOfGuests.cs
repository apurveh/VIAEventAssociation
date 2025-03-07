using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Common.Values;

public class NumberOfGuests : ValueObject
{
    private NumberOfGuests(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public static Result<NumberOfGuests> Create(int numberOfGuests)
    {
        try
        {
            var validation = Validate(numberOfGuests);
            if (validation.IsFailure)
                return validation.Error;
            return new NumberOfGuests(numberOfGuests);
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
    
    private static Result Validate(int numberOfGuests)
    {
        return numberOfGuests switch
        {
            < CONST.MIN_NUMBER_OF_GUESTS => Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS),
            > CONST.MAX_NUMBER_OF_GUESTS => Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS),
            _ => Result.Ok
        };
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}