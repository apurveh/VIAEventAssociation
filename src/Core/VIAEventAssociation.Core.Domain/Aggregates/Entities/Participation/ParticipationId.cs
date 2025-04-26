namespace ViaEventAssociation.Core.Domain.Entities;

public class ParticipationId : IdentityBase {
    private static readonly string PREFIX = "PID";

    private ParticipationId() : this(Guid.NewGuid().ToString()) { }
    private ParticipationId(string value) : base(PREFIX, value) { }


    public static Result<ParticipationId> GenerateId() {
        try {
            return new ParticipationId();
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<ParticipationId> Create(string value) {
        try {
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);

            if (value.Length != 39) errors.Add(Error.InvalidLength);

            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);

            if (errors.Any()) return Error.Add(errors);

            return new ParticipationId(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}