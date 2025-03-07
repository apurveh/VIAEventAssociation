using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Organizer;

public class Organizer : AggregateRoot<OrganizerId>
{
    private Organizer(OrganizerId id, OrganizerName name, Email email) : base(id)
    {
        OrganizerName = name;
        OrganizerEmail = email;
    }
    
    public OrganizerName OrganizerName { get; }
    public Email OrganizerEmail { get; }

    public static Result<Organizer> Create(string name, string email)
    {
        var errors = new HashSet<Error>();

        var organizerIdResult = OrganizerId.GenerateId();
        if (organizerIdResult.IsFailure)
            errors.Add(organizerIdResult.Error);
        
        var organizerNameResult = OrganizerName.Create(name);
        if (organizerNameResult.IsFailure)
            errors.Add(organizerNameResult.Error);
        
        var organizerEmailResult = Email.Create(email);
        if (organizerEmailResult.IsFailure)
            errors.Add(organizerEmailResult.Error);

        if (errors.Any())
            return Error.Add(errors);

        return new Organizer(organizerIdResult.Payload, organizerNameResult.Payload, organizerEmailResult.Payload);
    }
}