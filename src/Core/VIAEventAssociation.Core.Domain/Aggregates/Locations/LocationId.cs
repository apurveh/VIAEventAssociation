namespace ViaEventAssociation.Core.Domain.Agregates.Locations;

public class LocationId : IdentityBase {
    private static readonly string PREFIX = "LID";

    private LocationId() : this(Guid.NewGuid().ToString()) { }
    private LocationId(string value) : base(PREFIX, value) { }

    public static Result<LocationId> GenerateId() {
        try {
            return new LocationId();
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<LocationId> Create(string value) {
        try {
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);

            if (value.Length != 39) errors.Add(Error.InvalidLength);

            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);

            if (errors.Any()) return Error.Add(errors);

            return new LocationId(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}