using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class LocationId : IdentityBase
{
    private LocationId(string prefix) : base(prefix) { }

    public static Result<LocationId> GenerateId()
    {
        try
        {
            return new LocationId("LID");
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}