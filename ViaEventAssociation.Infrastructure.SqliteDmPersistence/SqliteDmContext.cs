using Microsoft.EntityFrameworkCore;using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Events; // adjust according to where your Event lives
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations; // same


namespace VIAEventAssociation.Infrastructure.SqliteDmPersistence
{
    public class SqliteDmContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Location> Locations { get; set; }

        public SqliteDmContext(DbContextOptions<SqliteDmContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDmContext).Assembly);
        }
    }

}