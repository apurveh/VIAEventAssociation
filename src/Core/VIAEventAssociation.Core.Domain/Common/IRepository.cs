using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common;

public interface IRepository<TAgg, in TId> where TAgg : AggregateRoot<TId> where TId : IdentityBase {
    Task<Result> AddAsync(TAgg aggregate);
    Task<Result> UpdateAsync(TAgg aggregate);
    Task<Result> DeleteAsync(TId id);
    Task<Result<TAgg>> GetByIdAsync(TId id);
    Task<Result<List<TAgg>>> GetAllAsync();
}