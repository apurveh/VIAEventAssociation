using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Organizer;

public class OrganizerName : ValueObject
{
    private OrganizerName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<OrganizerName> Create(string value)
    {
        try
        {
            var validation = Validate(value);
            return validation.IsSuccess ? new OrganizerName(value) : validation.Error;
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    private static Result Validate(string value)
    {
        var errors = new HashSet<Error>();
        
        // Validation
        
        return Result.Ok;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}