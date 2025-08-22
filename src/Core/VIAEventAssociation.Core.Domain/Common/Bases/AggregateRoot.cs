namespace ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : IdentityBase {
    protected AggregateRoot(TId id) : base(id) { }
    protected AggregateRoot(){}
}