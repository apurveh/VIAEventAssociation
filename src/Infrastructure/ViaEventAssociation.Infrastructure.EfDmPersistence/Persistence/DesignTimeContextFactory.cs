using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.Persistence;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<DmContext> {
    public DmContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<DmContext>();
        optionsBuilder.UseSqlite(@"Data Source = DBProduction.db");
        return new DmContext(optionsBuilder.Options);
    }
}