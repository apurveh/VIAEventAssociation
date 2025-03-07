using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Organizer;

public class OrganizerId : IdentityBase
{
    private OrganizerId(string prefix) : base(prefix)
    {
    }

    public static Result<OrganizerId> GenerateId()
    {
        try
        {
            return new OrganizerId("OID");
        }
        catch (Exception e)
        {
            return Error.FromException(e);
        }
    }
}