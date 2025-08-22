using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.Locations;

public class LocationName : ValueObject {
    private LocationName(string value) {
        Value = value;
    }

    public string Value { get; }

    public static Result<LocationName> Create(string name) {
        try {
            var validation = Validate(name);
            if (validation.IsFailure)
                return validation.Error;
            return new LocationName(name);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string name) {
        var errors = new HashSet<Error>();

        if (name == null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.BlankString);

        if (name.Length < CONST.MIN_NAME_LENGTH)
            errors.Add(Error.TooShortName(CONST.MIN_NAME_LENGTH));

        if (name.Length > CONST.MAX_NAME_LENGTH)
            errors.Add(Error.TooLongName(CONST.MAX_NAME_LENGTH));
        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}