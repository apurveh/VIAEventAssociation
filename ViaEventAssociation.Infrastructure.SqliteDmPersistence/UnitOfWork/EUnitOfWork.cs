using VIAEventAssociation.Core.Domain.Common.UnitOfWork;

namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly SqliteDmContext _context;

    public EfUnitOfWork(SqliteDmContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}