using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

namespace ViaEventAssociation.Infrastructure.EfcQueries;

public partial class DbproductionContext : DbContext
{
    public DbproductionContext()
    {
    }

    public DbproductionContext(DbContextOptions<DbproductionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source = C:\\VIA University\\Semester 6\\DCA1\\ViaEventAssociation\\src\\Infrastructure\\ViaEventAssociation.Infrastructure.EfDmPersistence\\DBProduction.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.EventEnd).HasColumnType("datetime");
            entity.Property(e => e.EventStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.ToTable("Participation");

            entity.HasIndex(e => e.EventId, "IX_Participation_EventId");

            entity.HasIndex(e => e.GuestId, "IX_Participation_GuestId");

            entity.HasOne(d => d.Event).WithMany(p => p.Participations).HasForeignKey(d => d.EventId);

            entity.HasOne(d => d.Guest).WithMany(p => p.Participations).HasForeignKey(d => d.GuestId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    public static DbproductionContext Seed(DbproductionContext context) {
        context.Guests.AddRange(GuestSeedFactory.CreateGuest());
        List<Event> events = EventSeedFactory.CreateEvents();
        context.Events.AddRange(events);
        context.SaveChanges();
        // ParticipationSeedFactory.Seed(context);
        // context.SaveChanges();
        // InvitationSeedFactory.Seed(context);
        // context.SaveChanges();
        return context;
    }

    public static DbproductionContext SetupContext() {
        var optionsBuilder = new DbContextOptionsBuilder<DbproductionContext>();
        var basePath = AppDomain.CurrentDomain.BaseDirectory; // Ensure the path is accessible
        var testDbName = $"TestDb_{Guid.NewGuid()}.db"; // Use GUID to ensure uniqueness
        var dataSource = Path.Combine(basePath, testDbName);
        optionsBuilder.UseSqlite($"Data Source={dataSource}");
        var context = new DbproductionContext(optionsBuilder.Options);
        context.Database.EnsureDeleted(); // Deletes the file if it exists
        context.Database.EnsureCreated(); // Creates a new file
        return context;
    }
}
