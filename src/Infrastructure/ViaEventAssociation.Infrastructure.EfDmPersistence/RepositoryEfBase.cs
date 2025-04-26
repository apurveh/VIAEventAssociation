namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence;

public abstract class RepositoryEfBase<T, TRepository>(DmContext context) {
    protected readonly DmContext _context = context;
}