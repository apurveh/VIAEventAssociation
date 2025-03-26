using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventId : IdentityBase
{
    private static readonly string PREFIX = "EID";
    private EventId() : base(PREFIX) { }
    private EventId(string value) : base(PREFIX, value) { }

    public static Result<EventId> GenerateId()
    {
        try
        {
            return new EventId();
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }

    public static Result<EventId> Create(string value)
    {
        try {
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);

            if (value.Length != 39) errors.Add(Error.InvalidLength);

            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);

            if (errors.Any()) return Error.Add(errors);

            return new EventId(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}