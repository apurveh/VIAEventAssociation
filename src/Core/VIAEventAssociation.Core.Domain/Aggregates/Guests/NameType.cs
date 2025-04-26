using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Agregates.Guests;

public class NameType : ValueObject {
    private static readonly int MAX_LENGTH = 25;
    private static readonly int MIN_LENGTH = 2;

    private NameType(string name) {
        Value = name;
    }

    private NameType() { }

    public string Value { get; }

    public static Result<NameType> Create(string name) {
        try {
            var validation = Validate(name);
            if (validation.IsFailure)
                return validation.Error;
            return new NameType(name);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string name) {
        var errors = new HashSet<Error>();

        if (name == null)
            return Error.NullString;

        if (string.IsNullOrEmpty(name))
            errors.Add(Error.BlankString);

        //if only contains letters, no numbers or special characters
        if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            errors.Add(Error.InvalidName);

        if (name.Length < MIN_LENGTH)
            errors.Add(Error.TooShortName(MIN_LENGTH));

        if (name.Length > MAX_LENGTH)
            errors.Add(Error.TooLongName(MAX_LENGTH));

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}