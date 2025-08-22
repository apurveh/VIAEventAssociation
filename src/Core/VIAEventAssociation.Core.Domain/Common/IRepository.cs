using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common;

public interface IRepository<TAgg, in TId> where TAgg : AggregateRoot<TId> where TId : IdentityBase {
    Task<Result> AddAsync(TAgg aggregate);
    Task<Result<TAgg>> GetByIdAsync(TId id);
 }