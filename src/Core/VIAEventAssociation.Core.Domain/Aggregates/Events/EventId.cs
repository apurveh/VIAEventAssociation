using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventId : IdentityBase
{
    private EventId(string prefix) : base(prefix) { }

    public static Result<EventId> GenerateId()
    {
        try
        {
            return new EventId("EID");
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}