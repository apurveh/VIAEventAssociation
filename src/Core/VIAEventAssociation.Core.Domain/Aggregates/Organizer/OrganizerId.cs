namespace ViaEventAssociation.Core.Domain.Agregates.Organizer;

public class OrganizerId : IdentityBase {
    private static readonly string PREFIX = "OID";

    private OrganizerId() : this(Guid.NewGuid().ToString()) { }
    private OrganizerId(string value) : base(PREFIX, value) { }

    public static Result<OrganizerId> GenerateId() {
        try {
            return new OrganizerId();
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<OrganizerId> Create(string value) {
        try {
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);

            if (value.Length != 39) errors.Add(Error.InvalidLength);

            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);

            if (errors.Any()) return Error.Add(errors);

            return new OrganizerId(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}