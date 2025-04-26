using Microsoft.EntityFrameworkCore;

public static class DbContextTestHelper {
    public static DmContext SetupContext() {
        var optionsBuilder = new DbContextOptionsBuilder<DmContext>();
        var basePath = AppDomain.CurrentDomain.BaseDirectory; // Ensure the path is accessible
        var testDbName = $"TestDb_{Guid.NewGuid()}.db"; // Use GUID to ensure uniqueness
        var dataSource = Path.Combine(basePath, testDbName);
        optionsBuilder.UseSqlite($"Data Source={dataSource}");
        var context = new DmContext(optionsBuilder.Options);
        context.Database.EnsureDeleted(); // Deletes the file if it exists
        context.Database.EnsureCreated(); // Creates a new file
        return context;
    }


    public static async Task SaveAndClearAsync<T>(T entity, DmContext context) where T : class {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}