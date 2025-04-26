namespace ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TId> where TId : ValueObject {
    protected Entity(TId id) {
        Id = id;
    }

    public TId Id { get; private set; }

    public override bool Equals(object? obj) {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (Entity<TId>)obj;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }


    public override string ToString() {
        return $"{nameof(Entity<TId>)}: {nameof(Id)}: {Id}";
    }
}