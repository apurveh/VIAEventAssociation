using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Agregates.Events;

public class EventTitle : ValueObject {
    private EventTitle(string title) {
        Value = title;
    }

    public EventTitle() { } // For EF Core

    public string Value { get; }

    public static Result<EventTitle> Create(string title) {
        try {
            var validation = Validate(title);
            if (validation.IsFailure)
                return validation.Error;
            return new EventTitle(title);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string title) {
        HashSet<Error> errors = new HashSet<Error>();

        if (title == null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(Error.BlankString);

        if (title.Length < CONST.MIN_TITLE_LENGTH)
            errors.Add(Error.TooShortTitle(CONST.MIN_TITLE_LENGTH));

        if (title.Length > CONST.MAX_TITLE_LENGTH)
            errors.Add(Error.TooLongTitle(CONST.MAX_TITLE_LENGTH));
        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}