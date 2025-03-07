namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;
        
        var other = (ValueObject) obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    
    protected abstract IEnumerable<object> GetEqualityComponents();
    
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
    
    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        
        return a.Equals(b);
    }
    
    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return string.Join(", ", GetEqualityComponents());
    }
}