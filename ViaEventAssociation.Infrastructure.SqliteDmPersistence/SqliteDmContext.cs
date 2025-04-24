using Microsoft.EntityFrameworkCore;using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Entities.Invitation;
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
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ParticipationId as a value object using a value converter
            modelBuilder.Entity<Participation>()
                .Property(p => p.Id)
                .HasConversion(
                    v => v.ToString(),  // Convert ParticipationId to string for storage
                    v => ParticipationId.Create(v));  // Convert string back to ParticipationId using the public factory method

            // Configure the relationship between Event and Participation (via ParticipationId)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participations)
                .WithOne()  // No back-reference in Participation
                .HasForeignKey("EventId");  // Assuming EventId is the foreign key in Participation

            // Configure inheritance for Participation (if applicable)
            modelBuilder.Entity<Participation>()
                .HasDiscriminator<string>("ParticipationType")  // Define the discriminator column
                .HasValue<Participation>("Participation")      // Define the base class
                .HasValue<Invitation>("Invitation");            // Define the subclass

            // Configure owned types for value objects in Event
            modelBuilder.Entity<Event>()
                .OwnsOne(e => e.EventTitle);

            modelBuilder.Entity<Event>()
                .OwnsOne(e => e.EventDescription);

            modelBuilder.Entity<Event>()
                .OwnsOne(e => e.EventTime);  // Assuming EventDateTime is a value object

            // If there are other relationships to handle (e.g., Guest, Location), configure them here
        }


    }
}