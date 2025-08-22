using ViaEventAssociation.Core.Domain.Agregates.Organizer;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Domain.Aggregates.Organizer;

public class Organizer : AggregateRoot<OrganizerId> {
    private Organizer(OrganizerId id, OrganizerName name, Email email) : base(id) {
        OrganizerName = name;
        OrganizerEmail = email;
    }

    //Required by EF Core
    private Organizer() : base(default!) { } // Required by EF Core

    public OrganizerName OrganizerName { get; private set; }
    public Email OrganizerEmail { get; private set; }

    public static Result<Organizer> Create(string name, string email) {
        HashSet<Error> errors = new HashSet<Error>();

        var organizerIdResult = OrganizerId.GenerateId();
        if (organizerIdResult.IsFailure)
            errors.Add(organizerIdResult.Error);

        var nameResult = OrganizerName.Create(name);
        if (nameResult.IsFailure)
            errors.Add(nameResult.Error);


        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            errors.Add(emailResult.Error);

        if (errors.Any())
            return Error.Add(errors);

        // Note: Directly passing validated domain objects to the constructor
        return new Organizer(organizerIdResult.Payload, nameResult.Payload, emailResult.Payload);
    }


    public Result<Event> CreateEvent() {
        var eventResult = Event.Create(this);
        return eventResult.IsSuccess ? eventResult.Payload : eventResult.Error;
    }
}