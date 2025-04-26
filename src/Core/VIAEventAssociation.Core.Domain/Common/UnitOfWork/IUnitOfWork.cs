namespace ViaEventAssociation.Core.Domain;

public interface IUnitOfWork {
    Task SaveChangesAsync();
}