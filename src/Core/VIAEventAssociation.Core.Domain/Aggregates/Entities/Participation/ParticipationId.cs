using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Entities;

public class ParticipationId : IdentityBase
{
    internal ParticipationId(string prefix) : base(prefix) { }
    
    public static ParticipationId Create(string prefix)
    {
        return new ParticipationId(prefix);
    }

    public static Result<ParticipationId> GenerateId()
    {
        try
        {
            return new ParticipationId("PID");
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}
