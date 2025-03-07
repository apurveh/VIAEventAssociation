using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDescription : ValueObject
{
    private EventDescription(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<EventDescription> Create(string value)
    {
        try
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return new EventDescription("");
            var validation = Validate(value);
            
            return validation.IsSuccess ? new EventDescription(value) : validation.Error;
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