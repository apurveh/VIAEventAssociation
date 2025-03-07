using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventTitle : ValueObject
{
    private EventTitle(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<EventTitle> Create(string value)
    {
        try
        {
            var validation = Validate(value);
            return validation.IsSuccess ? new EventTitle(value) : validation.Error;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    private static Result Validate(string value)
    {
        var errors = new HashSet<Error>();
        
        // Validations
        
        return Result.Ok;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}