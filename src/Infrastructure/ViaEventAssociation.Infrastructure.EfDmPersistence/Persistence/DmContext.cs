using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Aggregates.Entities.Invitation;
using ViaEventAssociation.Core.Domain.Aggregates.Entities.JoinRequest;
using ViaEventAssociation.Core.Domain.Aggregates.Entities.Participation;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Domain.Aggregates.Organizer;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

public class DmContext(DbContextOptions options) : DbContext(options) {
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmContext).Assembly);
        ConfigureGuest(modelBuilder.Entity<Guest>());
        ConfigureEvent(modelBuilder.Entity<Event>());
        ConfigureOrganizer(modelBuilder.Entity<Organizer>());
        ConfigureLocation(modelBuilder.Entity<Location>());
        ConfigureParticipation(modelBuilder.Entity<Participation>());
    }

    private void ConfigureParticipation(EntityTypeBuilder<Participation> entityBuilder) {
        entityBuilder.HasKey(p => p.Id); // Primary key

        entityBuilder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ParticipationId.Create(value).Payload
            );

        entityBuilder.HasDiscriminator<string>("Discriminator")
            .HasValue<JoinRequest>("JoinRequest")
            .HasValue<Invitation>("Invitation");

        // Configure the foreign key to Event
        entityBuilder.HasOne(p => p.Event)
            .WithMany(e => e.Participations)
            .HasForeignKey("EventId")
            .IsRequired(); //
    }


    private void ConfigureOrganizer(EntityTypeBuilder<Organizer> entity) {
        entity.HasKey(e => e.Id);

        entity
            .Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                dbValue => OrganizerId.Create(dbValue).Payload
            );

        entity.ComplexProperty<OrganizerName>(
            e => e.OrganizerName,
            e => {
                e.Property(e => e.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(100)
                    .IsRequired();
            }
        );

        entity.ComplexProperty<Email>(
            e => e.OrganizerEmail,
            e => {
                e.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(100)
                    .IsRequired();
            }
        );
    }


    private void ConfigureLocation(EntityTypeBuilder<Location> entity) {
        entity.HasKey(e => e.Id);

        entity
            .Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                dbValue => LocationId.Create(dbValue).Payload
            );
    }

    private void ConfigureEvent(EntityTypeBuilder<Event> entityBuilder) {
        entityBuilder.HasKey(e => e.Id);

        entityBuilder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => EventId.Create(value).Payload
            );


        entityBuilder.ComplexProperty<EventTitle>("Title",
            e => {
                e.Property(e => e.Value)
                    .HasColumnName("Title");
            });

        entityBuilder.ComplexProperty<EventDescription>("Description",
            e => {
                e.Property(e => e.Value)
                    .HasColumnName("Description");
            });

        entityBuilder.OwnsOne(
            typeof(EventDateTime),
            "TimeSpan", // This string is the name of the internal property
            od => {
                od.Property<DateTime>("Start").HasColumnName("EventStart");
                od.Property<DateTime>("End").HasColumnName("EventEnd");

                // Optionally, you can configure more details about each property
                od.Property<DateTime>("Start")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasColumnType("datetime")
                    .IsRequired();

                od.Property<DateTime>("End")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasColumnType("datetime");
            }
        );

        entityBuilder.Property<EventVisibility>("Visibility")
            .HasColumnName("Visibility")
            .HasConversion<string>();

        entityBuilder.Property<EventStatus>("Status")
            .HasColumnName("Status")
            .HasConversion<string>();

        entityBuilder.ComplexProperty<NumberOfGuests>("MaxNumberOfGuests",
            e => {
                e.Property(e => e.Value)
                    .HasColumnName("NumberOfGuests");
            });


        // Configure the collection relationship
        entityBuilder.HasMany(e => e.Participations)
            .WithOne(p => p.Event)
            .HasForeignKey("EventId"); // this should match your actual FK property in Participation
    }


    private void ConfigureGuest(EntityTypeBuilder<Guest> modelBuilder) {
        modelBuilder.HasKey(e => e.Id);

        modelBuilder
            .Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                dbValue => GuestId.Create(dbValue).Payload
            );

        modelBuilder
            .ComplexProperty<Email>(
                e => e.Email,
                e => {
                    e.Property(e => e.Value)
                        .HasColumnName("Email");
                }
            );

        modelBuilder
            .ComplexProperty<NameType>(
                e => e.FirstName,
                e => {
                    e.Property(e => e.Value)
                        .HasColumnName("FirstName");
                }
            );

        modelBuilder
            .ComplexProperty<NameType>(
                e => e.LastName,
                e => {
                    e.Property(e => e.Value)
                        .HasColumnName("LastName");
                }
            );
    }
}