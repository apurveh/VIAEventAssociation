using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class GuestId : IdentityBase
{ 
    private GuestId(string prefix) : base(prefix) { }

    public static Result<GuestId> GenerateId()
    {
        try
        {
            return new GuestId("GID");
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}