namespace ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class ValueObject {
    public override bool Equals(object? obj) {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    // This method is used to get the equality components of the value object. This method is used to compare the value objects.
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override int GetHashCode() {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }


    // Overriding the equality operators to compare value objects
    public static bool operator ==(ValueObject a, ValueObject b) {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return a.Equals(b);
    }

    // Overriding the inequality operators to compare value objects
    public static bool operator !=(ValueObject a, ValueObject b) {
        return !(a == b);
    }

    // Overriding the ToString method to return the value object as a string
    public override string ToString() {
        return string.Join(", ", GetEqualityComponents());
    }
}