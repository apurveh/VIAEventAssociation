using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence;

public abstract class RepositoryEfBase<TAgg, TId> : IRepository<TAgg, TId>
    where TAgg : AggregateRoot<TId> where TId : IdentityBase
{
    private readonly DbContext _context;

    protected RepositoryEfBase(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<Result> AddAsync(TAgg aggregate)
    {
        await _context.Set<TAgg>().AddAsync(aggregate);
        return await Task.FromResult(Result.Success());
    }

    public virtual async Task<Result<TAgg>> GetByIdAsync(TId id)
    {
        var root = await _context.Set<TAgg>().FindAsync(id);
        if (root == null)
        {
            if (typeof(TAgg) == typeof(Event))
            {
                return Result<TAgg>.Fail(Error.EventIsNotFound);
            }
            else if (typeof(TAgg) == typeof(Guest))
            {
                return Result<TAgg>.Fail(Error.GuestIsNotFound);
            }

            return Result<TAgg>.Fail(Error.RepoDoesnotExist);
        }
        return Result<TAgg>.Success(root);
    }
    
}