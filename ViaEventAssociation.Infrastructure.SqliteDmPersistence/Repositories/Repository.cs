using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Common;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Repositories
{
    public class Repository<TAgg, TId> : IRepository<TAgg, TId>
        where TAgg : AggregateRoot<TId>
        where TId : IdentityBase
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TAgg> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TAgg>();
        }

        public async Task<Result> AddAsync(TAgg aggregate)
        {
            try
            {
                await _dbSet.AddAsync(aggregate);
                return Result.Ok;
            }
            catch (Exception ex)
            {
                return Error.FromException(ex);
            }
        }

        public Task<Result<List<TAgg>>> GetAllAsync()
        {
            try
            {
                return Task.FromResult(Result<List<TAgg>>.Success(_dbSet.ToList()));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result<List<TAgg>>.Fail(Error.FromException(ex)));
            }
        }

        public async Task<Result<TAgg>> GetByIdAsync(TId id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity is null
                    ? Result<TAgg>.Fail(Error.GuestNotFound)
                    : Result<TAgg>.Success(entity);
            }
            catch (Exception ex)
            {
                return Error.FromException(ex);
            }
        }

        public Task<Result> UpdateAsync(TAgg aggregate)
        {
            try
            {
                _dbSet.Update(aggregate);
                return Task.FromResult(Result.Ok);
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result.Fail(Error.FromException(ex)));
            }
        }

        public async Task<Result> DeleteAsync(TId id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is null)
                    return Error.GuestNotFound;

                _dbSet.Remove(entity);
                return Result.Ok;
            }
            catch (Exception ex)
            {
                return Error.FromException(ex);
            }
        }
    }
}
