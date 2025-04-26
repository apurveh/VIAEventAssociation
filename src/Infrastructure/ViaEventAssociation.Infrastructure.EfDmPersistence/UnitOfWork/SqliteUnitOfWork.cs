using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

public class SqliteUnitOfWork(DbContext context) : IUnitOfWork {
    private readonly DbContext _context;

    public Task SaveChangesAsync() {
        return context.SaveChangesAsync();
    }
}