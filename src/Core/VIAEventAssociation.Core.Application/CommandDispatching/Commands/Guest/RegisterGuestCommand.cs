using VIAEventAssociation.Core.Domain.Common.Values;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class RegisterGuestCommand : ICommand<GuestId>
{
    public GuestId Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    private RegisterGuestCommand(GuestId id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<RegisterGuestCommand> Create(string guestIdAsString, string firstName, string lastName, string email)
    {
        var errors = new HashSet<Error>();

        var guestIdResult = GuestId.Create(guestIdAsString);
        if (guestIdResult.IsFailure)
            errors.Add(guestIdResult.Error);

        if (string.IsNullOrWhiteSpace(firstName))
            errors.Add(Error.BlankString);

        if (string.IsNullOrWhiteSpace(lastName))
            errors.Add(Error.BlankString);

        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankString);

        if (errors.Any())
            return Error.Add(errors);

        return new RegisterGuestCommand(guestIdResult.Payload, firstName, lastName, email);
    }
}