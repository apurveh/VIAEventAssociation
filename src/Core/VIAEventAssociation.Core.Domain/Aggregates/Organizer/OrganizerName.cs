using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.Organizer;

public class OrganizerName : ValueObject {
    private static readonly int MAX_LENGTH = 50;

    private OrganizerName(string value) {
        Value = value;
    }

    public string Value { get; }

    public static Result<OrganizerName> Create(string name) {
        try {
            var validation = Validate(name);
            if (validation.IsSuccess) return new OrganizerName(name);

            return validation.Error;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string name) {
        HashSet<Error> errors = new HashSet<Error>();

        if (name == null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.BlankString);

        if (name.Length > MAX_LENGTH)
            errors.Add(Error.TitleTooLong);

        if (errors.Any())
            return Error.Add(errors);

        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}