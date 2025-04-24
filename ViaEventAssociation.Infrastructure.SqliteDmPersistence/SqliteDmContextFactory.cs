namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class SqliteDmContextFactory : IDesignTimeDbContextFactory<SqliteDmContext>
{
    public SqliteDmContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqliteDmContext>();
        optionsBuilder.UseSqlite("Data Source=vea.db"); // Change this as needed

        return new SqliteDmContext(optionsBuilder.Options);
    }
}