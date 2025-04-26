using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Agregates.Events;

public class EventDescription : ValueObject {
    private static readonly int MIN_TITLE_LENGTH = 0;
    private static readonly int MAX_TITLE_LENGTH = 250;

    private EventDescription(string description) {
        Value = description;
    }

    public EventDescription() { } // For EF Core

    public string Value { get; }

    public static Result<EventDescription> Create(string description) {
        try {
            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
                return new EventDescription("");
            var validation = Validate(description);
            if (validation.IsFailure)
                return validation.Error;
            return new EventDescription(description);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<EventDescription> InitEmpty() {
        return Create("");
    }

    private static Result Validate(string description) {
        var errors = new HashSet<Error>();

        if (description.Length < MIN_TITLE_LENGTH)
            errors.Add(Error.TooShortDescription(MIN_TITLE_LENGTH));

        if (description.Length > MAX_TITLE_LENGTH)
            errors.Add(Error.TooLongDescription(MAX_TITLE_LENGTH));

        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}