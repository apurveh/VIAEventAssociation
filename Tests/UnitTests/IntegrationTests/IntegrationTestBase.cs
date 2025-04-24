using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Infrastructure.SqliteDmPersistence;

namespace UnitTests.IntegrationTests;
public abstract class IntegrationTestBase : IDisposable
{
    protected readonly SqliteDmContext Context;

    protected IntegrationTestBase()
    {
        var options = new DbContextOptionsBuilder<SqliteDmContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        Context = new SqliteDmContext(options);
        Context.Database.OpenConnection();  // Opens connection for SQLite memory DB
        Context.Database.EnsureCreated();   // Creates tables based on your model
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
