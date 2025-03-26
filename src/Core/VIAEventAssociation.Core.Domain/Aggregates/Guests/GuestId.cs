using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class GuestId : IdentityBase
{ 
    private static readonly string PREFIX = "GID";
    
    private GuestId() : base(PREFIX) { }
    private GuestId(string value) : base(PREFIX, value) { }

    public static Result<GuestId> GenerateId()
    {
        try
        {
            return new GuestId();
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public static Result<GuestId> Create(string value) {
        try {
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);

            if (value.Length != 39) errors.Add(Error.InvalidLength);

            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);

            if (errors.Any()) return Error.Add(errors);

            return new GuestId(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}