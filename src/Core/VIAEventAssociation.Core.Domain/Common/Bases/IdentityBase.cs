using ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class IdentityBase : ValueObject {
    protected IdentityBase(string prefix) {
        Value = prefix + Guid.NewGuid();
    }

    protected IdentityBase(string prefix, string value) {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public override string ToString() => Value;
}