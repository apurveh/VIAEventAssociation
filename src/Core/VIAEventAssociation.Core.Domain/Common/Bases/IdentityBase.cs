namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class IdentityBase : ValueObject
{
    public string Value { get; }
    
    protected IdentityBase(string value)
    {
        Value = value + Guid.NewGuid();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}