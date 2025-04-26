using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public class Email : ValueObject {
    private Email(string value) {
        Value = value;
    }

    private Email() { }
    public string Value { get; }

    public static Result<Email> Create(string email) {
        try {
            var validation = Validate(email);
            return validation.IsSuccess ? new Email(email) : validation.Error;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string email) {
        HashSet<Error> errors = new HashSet<Error>();

        if (email == null)
            return Error.NullString;

        // Check if email matches regular expression for email validation
        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankString);

        // Check if email matches regular expression for email validation
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            errors.Add(Error.InvalidEmail);

        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@via\.dk$"))
            errors.Add(Error.InvalidEmailDomain);

        // Separate local part from domain and check its length
        var localPart = email.Split('@')[0];
        if (localPart.Length < 3 || localPart.Length > 6)
            errors.Add(Error.InvalidEmail);

        // Check if local part is valid (3-4 letters or 6 digits)
        if (!(Regex.IsMatch(localPart, @"^[a-zA-Z]{3,4}$") || Regex.IsMatch(localPart, @"^\d{6}$")))
            errors.Add(Error.InvalidEmail);

        if (errors.Any())
            return Error.Add(errors);

        // If no errors, validation is successful
        return Result.Ok;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}