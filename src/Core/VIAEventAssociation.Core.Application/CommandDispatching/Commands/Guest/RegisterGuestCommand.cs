using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;

namespace ViaEventAssociation.Core.Application.Features.Commands.Guest;

public class RegisterGuestCommand : Command<GuestId> {
    private RegisterGuestCommand(GuestId guestId, NameType firstName, NameType lastName, Email email) : base(guestId) {
        GuestId = guestId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public GuestId GuestId { get; }
    public NameType FirstName { get; }
    public NameType LastName { get; }
    public Email Email { get; }

    public static Result<RegisterGuestCommand> Create(string firstName, string lastName, string email) {
        var errors = new HashSet<Error>();

        var Guid = GuestId.GenerateId()
            .OnFailure(error => errors.Add(error));

        var FirstName = NameType.Create(firstName)
            .OnFailure(error => errors.Add(error));

        var LastName = NameType.Create(lastName)
            .OnFailure(error => errors.Add(error));

        var Mail = Email.Create(email)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new RegisterGuestCommand(Guid.Payload, FirstName.Payload, LastName.Payload, Mail.Payload);
    }
}